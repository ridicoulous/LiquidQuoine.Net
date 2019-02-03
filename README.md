# ![Icon](https://github.com/ridicoulous/LiquidQuoine.Net/blob/master/Resources/icon.png?raw=true) LiquidQuoine.Net 

![Build status](https://travis-ci.org/ridicoulous/LiquidQuoine.Net.svg?branch=master)

LiquidQuoine.Net is a .Net wrapper for the liquid.com (by Quoine) API as described on [documentation](https://developers.quoine.com/). It includes all features the API provides using clear and readable C# objects including 
* Reading market info
* Placing and managing orders
* Reading accounts, balances and funds

Additionally it adds some convenience features like:
* Configurable rate limiting
* Autmatic logging

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/ridicoulous/LiquidQuoine.Net/issues)**

---
Also check out other exchange API wrappers based on [JKorf's abstraction CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net):
<table>

<tr>
<td><a href="https://github.com/JKorf/Bitfinex.Net"><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a>
</td>
<td><a href="https://github.com/JKorf/Binance.Net"><img src="https://github.com/JKorf/Binance.Net/blob/master/Resources/binance-coin.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Binance.Net">Binance</a>
</td>
<td><a href="https://github.com/JKorf/Bittrex.Net"><img src="https://github.com/JKorf/Bittrex.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bittrex.Net">Bittrex</a>
</td>
<td><a href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/CoinEx.Net">CoinEx</a>
</td>
<td><a href="https://github.com/JKorf/Huobi.Net"><img src="https://github.com/JKorf/Huobi.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Huobi.Net">Huobi</a>
</td>
<td><a href="https://github.com/Zaliro/Switcheo.Net"><img src="https://github.com/Zaliro/Switcheo.Net/blob/master/Resources/switcheo-coin.png?raw=true"></a>
<br />
<a href="https://github.com/Zaliro/Switcheo.Net">Switcheo</a>
</tr>
</table>

## Donations
Donations are greatly appreciated and a motivation to keep improving.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  

## Installation
![Nuget version](https://img.shields.io/nuget/v/liquidquoine.net.svg) ![Nuget downloads](https://img.shields.io/nuget/dt/LiquidQuoine.Net.svg)

Available on [NuGet](https://www.nuget.org/packages/LiquidQuoine.Net/):
```
PM> Install-Package LiquidQuoine.Net
```
To get started with LiquidQuoine.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using [NuGet](https://www.nuget.org/packages/LiquidQuoine.Net/). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'LiquidQuoine.Net' and hit enter. The LiquidQuoine.Net package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use LiquidQuoine.Net in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that LiquidQuoine.Net will be installed in. After selecting the correct project type  `Install-Package LiquidQuoine.Net`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using LiquidQuoine.Net.

## Getting started
After  it's time to actually use it. To get started we have to add the LiquidQuoine.Net namespace:  `using LiquidQuoine.Net;`.

LiquidQuoine.Net provides client to interact with the Liquid.com API. The `LiquidQuoineClient` provides all rest API calls. 

Most API methods are available in two flavors, sync and async:
````C#
public void NonAsyncMethod()
{
    using(var client = new LiquidQuoineClient())
    {
        var result = client.GetAllProducts();
    }
}

public async Task AsyncMethod()
{
    using(var client = new LiquidQuoineClient())
    {
        var result2 = await client.GetAllProductsAsync();
    }
}
````

## Response handling
All API requests will respond with an CallResult object. This object contains whether the call was successful, the data returned from the call and an error if the call wasn't successful. As such, one should always check the Success flag when processing a response.
For example:
````C#
using(var client = new LiquidQuoineClient())
{
	var orderBookByProductId = client.GetOrderBook(5);
	if (priceResult.Success)
		Console.WriteLine($"OrderBook: {orderBookByProductId.Data.BuyPriceLevels[0].Price}");
	else
		Console.WriteLine($"Error: {priceResult.Error.Message}");
}
````

## Options & Authentication
The default behavior of the clients can be changed by providing options to the constructor, or using the `SetDefaultOptions` before creating a new client to set options for all new clients. Api credentials can be provided in the options.



## Release notes

* Version 0.0.1 - 03 feb 2019
	* Initial release