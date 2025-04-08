package com.abstractfactory.furniture;

public class VictorianFurnitureFactory extends AbstractFurnitureFactory {

	@Override
	public void createCoffeeTable() {
		// TODO Auto-generated method stub
		coffeeTable = new VictorianCoffeeTable();
	}

	@Override
	public void createChair() {
		// TODO Auto-generated method stub
		chair = new VictorianChair();
	}

	@Override
	public void createSofa() {
		// TODO Auto-generated method stub
		sofa = new VictorianSofa();
	}

}
