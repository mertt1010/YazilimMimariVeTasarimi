package com.abstractfactory.furniture;

public class MainProgram {
	AbstractFurnitureFactory furnitureFactory;
	Chair chair;
	Sofa sofa;
	CoffeeTable coffeeTable;
	public static void main(String[] args) {
		
		new MainProgram();
		
	}
	
	public MainProgram() {
		
		
		// firstly create victorian style furnitures
		furnitureFactory = new VictorianFurnitureFactory();
		furnitureFactory.createChair();
		furnitureFactory.createSofa();
		furnitureFactory.createCoffeeTable();
		chair=furnitureFactory.chair;
		sofa=furnitureFactory.sofa;
		coffeeTable=furnitureFactory.coffeeTable;
		chair.render();
		sofa.render();
		coffeeTable.render();
		
		// then ArtDeco
		furnitureFactory = new ArtDecoFurnitureFactory(); // Yukarıdakinden farkı sadece Factorynin türü değişmektedir.
		furnitureFactory.createChair();
		furnitureFactory.createSofa();
		furnitureFactory.createCoffeeTable();
		chair=furnitureFactory.chair;
		sofa=furnitureFactory.sofa;
		coffeeTable=furnitureFactory.coffeeTable;
		chair.render();
		sofa.render();
		coffeeTable.render();
		
		// then Modern
		furnitureFactory = new ModernFurnitureFactory(); // Yukarıdakinden farkı sadece Factorynin türü değişmektedir.
		furnitureFactory.createChair();
		furnitureFactory.createSofa();
		furnitureFactory.createCoffeeTable();
		chair=furnitureFactory.chair;
		sofa=furnitureFactory.sofa;
		coffeeTable=furnitureFactory.coffeeTable;
		chair.render();
		sofa.render();
		coffeeTable.render();
		
		System.out.println("\n\n-------------------------\n\n");
		
		// Now, let's create a generalized function which takes factory as parameter and call it for furniture creation.
		createFurnituresAccordingToFactory(new VictorianFurnitureFactory());
		createFurnituresAccordingToFactory(new ArtDecoFurnitureFactory());
		createFurnituresAccordingToFactory(new ModernFurnitureFactory());
	}

	private void createFurnituresAccordingToFactory(AbstractFurnitureFactory furnitureFactory) {
		// TODO Auto-generated method stub
		
		furnitureFactory.createChair();
		furnitureFactory.createSofa();
		furnitureFactory.createCoffeeTable();
		chair=furnitureFactory.chair;
		sofa=furnitureFactory.sofa;
		coffeeTable=furnitureFactory.coffeeTable;
		chair.render();
		sofa.render();
		coffeeTable.render();
	}
}
