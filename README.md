Automate the inventory management based on the provided rules.

Clone the repository > Open Solution > Run

First screen (/Inventory/index) displays the list of inventory items. The current date is also present at the top of the page. 

Clicking the blue button "Adjust Quality & Sellin-Days" mimics a day passing. Clicking this button will update the inventory stock and increase the current date by one.

A single item in the inventory represents an InventoryItem.cs within the Models folder. I then have an Inventory.cs which contains a list of InventoryItems. The UI used to display the data can be found within Views/Inventory/Index.

I create a list and pass it in to the view within the InventoryController. Here I also call the "Update" method every button clicks, which allows me to pass back the list to the action, change the values, and pass back to the view for visual changes.

The ui and styles was just a quick mock up to display the inventory. I've discussed all methods and reasons for using them within the project files. I'm also just creating the list in the code behind, in a real environtment it would obviously come from a database etc. I've used .NET CORE MVC and Bootstrap 4.
