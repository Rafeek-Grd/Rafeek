using Bogus;
using System;
using System.Linq;
using System.Reflection;
using Rafeek.Domain.Entities;

class Program {
    static void Main() {
        var assembly = typeof(BaseEntity).Assembly;
        var entities = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseEntity)) && !t.IsAbstract).ToList();
        foreach(var e in entities) {
            Console.WriteLine($"var faker{e.Name} = new Faker<{e.Name}>(\"en\")");
            foreach(var p in e.GetProperties()) {
                if(p.PropertyType == typeof(Guid)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => Guid.NewGuid())");
                else if(p.PropertyType == typeof(string)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => f.Lorem.Word())");
                else if(p.PropertyType == typeof(int)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => f.Random.Int(1, 10))");
                else if(p.PropertyType == typeof(bool)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => f.Random.Bool())");
                else if(p.PropertyType == typeof(DateTime)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => DateTime.UtcNow)");
                else if(p.PropertyType == typeof(float)) Console.WriteLine($"    .RuleFor(x => x.{p.Name}, f => f.Random.Float(1, 4))");
            }
            Console.WriteLine("    .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)");
            Console.WriteLine("    .RuleFor(x => x.CreatedBy, f => \"Seeder\")");
            Console.WriteLine("    .RuleFor(x => x.IsActive, f => true);");
            Console.WriteLine($"var {e.Name.ToLower()}List = faker{e.Name}.Generate(10);");
            Console.WriteLine($"context.{e.Name}s.AddRange({e.Name.ToLower()}List);");
        }
    }
}
