package com.abstractfactory.furniture;

public class ArtDecoFurnitureFactory extends AbstractFurnitureFactory {

	@Override
	public void createCoffeeTable() {
		// TODO Auto-generated method stub
		coffeeTable = new ArtDecoCoffeeTable();
	}

	@Override
	public void createChair() {
		// TODO Auto-generated method stub
		chair = new ArtDecoChair();
	}

	@Override
	public void createSofa() {
		// TODO Auto-generated method stub
		sofa = new ArtDecoSofa();
	}

}
