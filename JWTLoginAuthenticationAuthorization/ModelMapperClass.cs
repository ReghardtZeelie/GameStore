using Models;

namespace GameStore
{
    public class ModelMapperClass
    {
        public ItemsModel MapNewItemModelToItemsModel(NewItemModel newItem, byte[] image)
        {
           
            ItemsModel item = new ItemsModel();
            item.Id = 0;
            item.ItemName = newItem.ItemName.Trim();
            item.ItemDescription = newItem.ItemDescription.Trim();
            item.itemCost = newItem.itemCost;
            item.ItemWholeSale = newItem.ItemWholeSale;
            item.ImageFile = image;
            item.Make = newItem.Make.Trim();
            item.Model = newItem.Model.Trim();
            item.FileName = newItem.file.FileName;
            item.fileType = Path.GetExtension(newItem.file.FileName);
            return item;
        }

    }
}
