using ToyStore.API.Interfaces;
using ToyStore.API.Models;

namespace ToyStore.API.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public IEnumerable<Product> GetAll() => _products.AsReadOnly();

    public Product? GetById(int id) => _products.Find(p => p.Id == id);

    public void Add(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
    }

    public bool Update(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index == -1) return false;

        _products[index] = product;
        return true;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        return product != null && _products.Remove(product);
    }
}