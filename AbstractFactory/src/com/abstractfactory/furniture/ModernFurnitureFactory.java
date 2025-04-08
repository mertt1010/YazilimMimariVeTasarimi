package com.abstractfactory.furniture;

public class ModernFurnitureFactory extends AbstractFurnitureFactory {

	@Override
	public void createCoffeeTable() {
		// TODO Auto-generated method stub
		coffeeTable = new ModernCoffeeTable();
	}

	@Override
	public void createChair() {
		// TODO Auto-generated method stub
		chair = new ModernChair();
	}

	@Override
	public void createSofa() {
		// TODO Auto-generated method stub
		sofa = new ModernSofa();
	}

}
