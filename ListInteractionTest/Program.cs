using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//imported these manually 
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint;
using sp_rwu8;

namespace ListInteractionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleDataContext context = new SampleDataContext("http://localhost/raymond");
            while(true)
            {
                Console.WriteLine("Press 1 to see all items on the list. ");
                Console.WriteLine("Press 2 to add a new item to the list");
                Console.WriteLine("Press 3 to update an item on the list");
                Console.WriteLine("Press 4 to delete an item on the list");
                Console.WriteLine("Press 5 to exit the program");

                Console.Write("Input: ");
                String input = Console.ReadLine();

                if (input.Equals("1"))
                {
                    showListItems(context);
                    Console.ReadKey(true);
                }
                else if (input.Equals("2"))
                {
                    Console.Write("Enter a first name: ");
                    String first_name = Console.ReadLine();
                    Console.Write("Enter a last name: ");
                    String last_name = Console.ReadLine();
                    insertListItem(context, first_name, last_name);
                    Console.WriteLine("Success! ");
                    Console.ReadKey(true);
                }
                else if (input.Equals("3"))
                {
                    showListItems(context);
                    Console.Write("Enter the ID of the product you wish to update: ");
                    int updateInput = Int32.Parse(Console.ReadLine());
                    Console.Write("Enter a new first name: ");
                    String newFirst = Console.ReadLine();
                    Console.Write("Enter a new last name: ");
                    String newLast = Console.ReadLine();

                    var itemToUpdate = (from r in context.Sample where r.Id == updateInput select r).First();
                    itemToUpdate.FirstName = newFirst;
                    itemToUpdate.LastName = newLast;
                    context.SubmitChanges();

                    Console.WriteLine("Success! ");
                    Console.ReadKey(true);
                }
                else if (input.Equals("4"))
                {
                    showListItems(context);
                    Console.Write("Enter the ID of the product you wish to delete: ");
                    int updateInput = Int32.Parse(Console.ReadLine());
                    deleteItem(context, updateInput);

                    Console.WriteLine("Success! ");
                    Console.ReadKey(true);
                }
                else if (input.Equals("5"))
                {
                    break;
                }
            }
           
            
        }

        private static void showListItems(SampleDataContext context)
        {
            //context is the site (in this case this would be the subsite, Raymond) 
            var res = from r in context.Sample select r;
            foreach (var r in res)
            {
                Console.WriteLine(r.Id + ":" + r.FirstName + ":" + r.LastName);
            }
        }

        private static void insertListItem(SampleDataContext context, String first_name, String last_name)
        {
            //Refer to Sample.cs
            //Recall this is generated from SPMetal.exe
            EntityList<SampleItem> samples = context.GetList<SampleItem>("Sample");
            SampleItem newItem = new SampleItem()
            {
                FirstName = first_name,
                LastName = last_name
            };
            samples.InsertOnSubmit(newItem);
            context.SubmitChanges();
        }

        private static void deleteItem(SampleDataContext context, int updateInput)
        {
            EntityList<SampleItem> samples = context.GetList<SampleItem>("Sample");
            var itemToDelete = (from r in context.Sample where r.Id == updateInput select r).First();
            samples.DeleteOnSubmit(itemToDelete);
            context.SubmitChanges();
        }
    }
}
