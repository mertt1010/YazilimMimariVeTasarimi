package com.abstractfactory.furniture;

public abstract class AbstractFurnitureFactory {
	public CoffeeTable coffeeTable;
	public Chair chair;
	public Sofa sofa;
	
	public abstract void createCoffeeTable();
	public abstract void createChair();
	public abstract void createSofa();
}
