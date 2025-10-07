Feature: ShopCart

A short summary of the feature
Here can be Background done for every test case

@smoke
@shopCart
Scenario: Item can be added to the cart successfuly
	Given User clicked on any product item
	And click on 'Add to cart' button
	When user opens cart
	Then product is displayed in the cart
