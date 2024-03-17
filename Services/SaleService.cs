using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Service_.Net_Core.Models;
using Web_Service_.Net_Core.Models.ApiResponse;
using Web_Service_.Net_Core.Models.Request;
using Web_Service_.Net_Core.Models.Tools;

namespace Web_Service_.Net_Core.Services
{
    public class SaleService : ISaleService
    {
        public readonly DataContext _context;

        public SaleService(DataContext dBContext)
        {
            _context = dBContext;

        }
        public ApiResponse<Sale> AddSale(SaleRequest oSaleRequest)
        {
            Console.WriteLine("Añadiendo venta");
            Console.WriteLine(oSaleRequest.IdClient);
            Console.WriteLine(oSaleRequest.Concepts.Count);
            Console.WriteLine(oSaleRequest.Date);
            foreach (var item in oSaleRequest.Concepts)
            {
                Console.WriteLine(item.IdProduct);
                Console.WriteLine(item.Quantity);
                Console.WriteLine(item.UnitaryPrice);
                Console.WriteLine(item.Import);
            }
            Console.WriteLine("AFecha de venta: " + oSaleRequest.Date);
            using var dbTransaction = _context.Database.BeginTransaction();

            try
            {
                var venta = new Sale
                {
                    Total = oSaleRequest.Concepts.Sum(x => x.Quantity * x.UnitaryPrice),
                    Date = oSaleRequest.Date ?? DateTime.Now,
                    IdClient = oSaleRequest.IdClient,
                    State = 1
                };
                _context.Sales.Add(venta);
                _context.SaveChanges();

                dbTransaction.CreateSavepoint("BeforeAddingConcepts");

                foreach (var item in oSaleRequest.Concepts)
                {
                    Concept concept = new()
                    {
                        IdSale = venta.Id,
                        IdProduct = item.IdProduct,
                        Quantity = item.Quantity,
                        UnitaryPrice = item.UnitaryPrice,
                        Import = item.Quantity * item.UnitaryPrice,
                        State = 1
                    };
                    _context.Concepts.Add(concept);
                    var product = _context.Products.Single(p => p.Id == item.IdProduct);
                    product.Stock -= item.Quantity;
                }

                _context.SaveChanges();
                dbTransaction.Commit();

                Sale? oSale = _context.Sales
                          .Include(x => x.Client)
                          .Include(x => x.Concepts)
                          .FirstOrDefault(x => x.Id == venta.Id);
                return new ApiResponse<Sale>
                {
                    Success = 1,
                    Message = "Se agrego correctamente la venta",
                    Data = oSale != null ? [oSale] : [],
                    TotalCount = 1
                };
            }
            catch (Exception e)
            {
                dbTransaction.RollbackToSavepoint("BeforeAddingConcepts");
                throw new Exception("No se pudo realizar la venta" + e);
            }
        }


        public ApiResponse<Sale> DeleteSale(long Id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Find the Sale
                    Sale? oSale = _context.Sales
                                        .Where(v => v.Id == Id && v.State == 1)
                                        .FirstOrDefault()
                                        ?? throw new Exception("No se encontró una Sale activa con ese ID");

                    // Set the State to false for the Sale
                    oSale.State = 0;
                    _context.Entry(oSale).State = EntityState.Modified;

                    // Find related objects (assuming ObjetoConcept with IdSale property)
                    var concepts = _context.Concepts
                                                       .Where(c => c.IdSale == Id && c.State == 1)
                                                       .ToList();

                    // Set the State to false for each related ObjetoConcept
                    foreach (var concept in concepts)
                    {
                        concept.State = 1;
                        _context.Entry(concept).State = EntityState.Modified;
                    }

                    // Save changes to the database
                    _context.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    return new ApiResponse<Sale>
                    {
                        Success = 1,
                        Message = "Sale y objetos relacionados eliminados correctamente",
                        Data = null,
                        TotalCount = 1
                    };
                }
                catch (Exception)
                {
                    // An error occurred, rollback the transaction
                    transaction.Rollback();
                    throw; // Re-throw the exception to propagate it
                }
            }
        }

        public ApiResponse<Sale> FullDeleteSale(long Id)
        {
            Sale? oSale = _context.Sales.Find(Id) ?? throw new Exception("No se encontro la Sale");
            _context.Remove(oSale);
            _context.SaveChanges();

            return new ApiResponse<Sale>
            {
                Success = 1,
                Message = "Sale eliminada correctamente",
                Data = null,
                TotalCount = 1
            };
        }

        public ApiResponse<Sale> GetSale(long Id)
        {
            Sale? oSale = _context.Sales
                              .Include(x => x.Client)
                              .Include(x => x.Concepts)
                              .ThenInclude(c => c.Product)
                              .Where(v => v.Id == Id && v.State == 1)
                              .FirstOrDefault()
                              ?? throw new Exception("No se encontro una venta activa con ese ID");
            //   oSale.Concepts= [.. _context.Concepts.Where(c => c.IdSale == Id)];

            return new ApiResponse<Sale>
            {
                Success = 1,
                Message = "Sale obtenida correctamente",
                Data = [oSale],
                TotalCount = 1
            };
        }
        public ApiResponse<Sale> GetSales(QueryParameters queryParameters)
        {
            Type type = queryParameters.GetType();

            // Obtener todas las propiedades públicas de QueryParameters
            PropertyInfo[] properties = type.GetProperties();

            // Iterar sobre todas las propiedades e imprimir sus nombres y valores
            foreach (var property in properties)
            {
                object value = property.GetValue(queryParameters);
                Console.WriteLine($"{property.Name}: {value}");
            }


            IQueryable<Sale> query = _context.Sales
                .Include(v => v.Client)
                .Include(v => v.Concepts)
                .ThenInclude(c => c.Product);



            // Add a condition to filter clients with State equal to 1
            query = query.Where(p => p.State == 1);

            if (!string.IsNullOrEmpty(queryParameters.Filter))
            {
                string filter = queryParameters.Filter.ToLower();
                // DateTime.TryParse(queryParameters.Filter, out DateTime filterDate);
                string[] filterWords = filter.Split(' ');
                query = query.Where(v =>
                       //    v.Date.Date == filterDate.Date ||
                       v.Client.Name.ToLower().Contains(filter) ||
                       v.Client.LastName.ToLower().Contains(filter) ||
                       (v.Client.LastName.ToLower() + " " + v.Client.Name.ToLower()).Contains(filter) ||
                       (v.Client.Name.ToLower() + " " + v.Client.LastName.ToLower()).Contains(filter) ||
                       filterWords.Contains(v.Client.Name.ToLower()) ||
                       filterWords.Contains(v.Client.LastName.ToLower()) ||
                       filterWords.Contains(v.Client.LastName.ToLower() + ' ' + v.Client.Name.ToLower()) ||
                       filterWords.Contains(v.Client.Name.ToLower() + ' ' + v.Client.LastName.ToLower()) ||
                       v.Concepts.Any(c => c.Product.Name.ToLower().Contains(filter))
                   );

            }

            if (queryParameters.StartDate.HasValue && queryParameters.EndDate.HasValue)
            {
                // Filtrar las ventas cuyas fechas estén dentro del rango especificado
                query = query.Where(v => v.Date >= queryParameters.StartDate && v.Date <= queryParameters.EndDate);
            }
            else if (queryParameters.StartDate.HasValue)
            {
                // Filtrar las ventas cuyas fechas sean posteriores o iguales a la fecha de inicio
                query = query.Where(v => v.Date >= queryParameters.StartDate);
            }
            else if (queryParameters.EndDate.HasValue)
            {
                // Filtrar las ventas cuyas fechas sean anteriores o iguales a la fecha de fin
                query = query.Where(v => v.Date <= queryParameters.EndDate);
            }


            var totalElements = query.Count();

            // Add an OrderBy clause to make the query predictable
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                string orderByProperty = queryParameters.OrderBy.ToLower();
                query = orderByProperty switch
                {
                    "id" => query.OrderBy(v => v.Id),
                    "name" => query.OrderBy(v => v.Client.Name),
                    "lastname" => query.OrderBy(v => v.Client.LastName),
                    "date" => query.OrderBy(v => v.Date),
                    "total" => query.OrderBy(v => v.Total),
                    _ => query.OrderBy(v => v.Id),
                };
                if (queryParameters.Desc == 1)
                {
                    query = query.Reverse(); // This assumes Reverse is a valid extension method for IQueryable (you may need to implement it)
                }
            }

            if (queryParameters.Skip.HasValue)
            {
                query = query.Skip(queryParameters.Skip.Value);
            }

            if (queryParameters.Limit.HasValue)
            {
                query = query.Take(queryParameters.Limit.Value);
            }



            var sales = query.ToList();

            if (sales.Count == 0) throw new Exception("No se encontraron clientes");

            return new ApiResponse<Sale>
            {
                Success = 1,
                Message = "Sales obtenidos correctamente",
                Data = sales,
                TotalCount = totalElements
            };
        }


        // }

        //TODO: Probar bien este metodo que esta raro xD
        public ApiResponse<Sale> UpdateSale(SaleRequest oSaleRequest)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Sale? oSale = _context.Sales
                       .Include(v => v.Concepts)  // Include Concepts for updating
                       .FirstOrDefault(v => v.Id == oSaleRequest.Id)
                       ?? throw new Exception("No se encontró una Sale con ese ID");

                    oSale.Total = oSaleRequest.Concepts.Count > 0 ? oSaleRequest.Concepts.Sum(x => x.Quantity * x.UnitaryPrice) : oSale.Total;
                    oSale.Date = oSaleRequest.Date ?? oSale.Date;
                    oSale.IdClient = oSaleRequest.IdClient != 0 ? oSaleRequest.IdClient : oSale.IdClient;
                    oSale.State = 1;

                    _context.Entry(oSale).State = EntityState.Modified;
                    _context.SaveChanges();

                    var existingConcepts = _context.Concepts.Where(ec => ec.IdSale == oSaleRequest.Id).ToList();
                    foreach (var existingConcept in existingConcepts)
                    {
                        // Verificar si el concepto existente no está en la nueva venta
                        if (!oSaleRequest.Concepts.Any(updatedConcept => updatedConcept.Id == existingConcept.Id))
                        {
                            // Si no está en la nueva venta, eliminarlo de la base de datos
                            _context.Entry(existingConcept).State = EntityState.Deleted;
                        }
                    }


                    foreach (var updatedConcept in oSaleRequest.Concepts)
                    {
                        var existingConcept = _context.Concepts.FirstOrDefault(ec => ec.Id == updatedConcept.Id);

                        if (existingConcept != null)
                        {

                            int quantity = updatedConcept.Quantity - existingConcept.Quantity;

                            // Update properties of the existing Concept
                            existingConcept.IdProduct = updatedConcept.IdProduct;
                            existingConcept.Quantity = updatedConcept.Quantity;
                            existingConcept.UnitaryPrice = updatedConcept.UnitaryPrice;
                            existingConcept.Import = updatedConcept.Quantity * updatedConcept.UnitaryPrice;
                            _context.Entry(existingConcept).State = EntityState.Modified;

                            var product = _context.Products.Single(p => p.Id == updatedConcept.IdProduct);
                            Console.WriteLine("El stock de ese producto es: " + product.Stock);
                            Console.WriteLine("La cantidad es: " + quantity);
                            product.Stock -= quantity;
                        }
                        else
                        {
                            Concept concept = new()
                            {
                                IdSale = oSaleRequest.Id,
                                IdProduct = updatedConcept.IdProduct,
                                Quantity = updatedConcept.Quantity,
                                UnitaryPrice = updatedConcept.UnitaryPrice,
                                Import = updatedConcept.Quantity * updatedConcept.UnitaryPrice,
                                State = 1
                            };
                            _context.Entry(concept).State = EntityState.Added;
                            var product = _context.Products.Single(p => p.Id == updatedConcept.IdProduct);
                            product.Stock -= updatedConcept.Quantity;
                        }
                    }
                    _context.SaveChanges();
                    dbTransaction.Commit();

                    Sale? oSaleRes = _context.Sales
                              .Include(x => x.Client)
                              .Include(x => x.Concepts)
                              .FirstOrDefault(x => x.Id == oSale.Id);
                    return new ApiResponse<Sale>
                    {
                        Success = 1,
                        Message = "Se edito correctamente la venta",
                        Data = oSaleRes != null ? [oSaleRes] : [],
                        TotalCount = 1
                    };
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception("No se pudo editar la venta" + e);
                }
            }
        }
    }
}
