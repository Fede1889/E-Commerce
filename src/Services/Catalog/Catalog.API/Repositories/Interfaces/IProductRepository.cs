using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();                               //restituisce tutti i prodotti presenti
        Task<Product> GetProduct(string id);                                    //restituisce tutti i prodotti con ID = id
        Task<IEnumerable<Product>> GetProductByName(string name);               //restituisce tutti i prodotti con NAME = name
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);   //restituisce tutti i prodotti con CATEGORY = categoryName

        Task<bool> CreateProduct(Product product);        //ci permette di inserire nel database un nuovo prodotto
        Task<bool> UpdateProduct(Product product);  //ci permette di aggiornare le informazioni riguardanti product
        Task<bool> DeleteProduct(string id);        //ci permette di eliminare il prodotto con ID = id
    }
}
