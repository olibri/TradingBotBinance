$(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/price") // URL of the SignalR hub
        .build();

    // Start the connection
    connection.start()
        .then(function () {
            console.log("Connected to the SignalR hub!");
            connection.invoke("PriceGet");
        })
        .catch(function (err) {
            return console.error(err.toString());
        });

    connection.on("Receive", function (priceModel) {
        // console.log(priceModel);
        document.getElementById("symbolDisplay").innerText = "Symbol: " + priceModel.s + ", Price: " ;
        document.getElementById("priceDisplay").innerText = priceModel.p;
    });
});



document.getElementById("priceDisplay").addEventListener("click", function () {
    console.log("click");
    var price = this.innerText; 
    document.getElementById("priceInput").value = price; 
});

document.getElementById('shortButton').addEventListener('click', function () {
    sendPrice("SHORT", "SELL");
});

document.getElementById('longButton').addEventListener('click', function () {
    sendPrice("LONG", "BUY");
});

function sendPrice(_actionType, _side) {

    var _price = document.getElementById('priceInput').value;
    var _quantity = document.getElementById('quantityInput').value;

    var newOrder = {
        //symbol: "DOGEUSDT",
        side: _side, 
        //positionSide: _actionType,
        type: "STOP",
        quantity: _quantity,
        price: _price,
        stopPrice: _price, //for test 
        //activationPrice: price, 
    };
    console.log(_quantity); 

    fetch('/SetOrder/Post',
    {       
        method: 'POST',
        headers:
        {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newOrder)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert(data.message);
            getItems();
            addNameTextbox.value = '';
        } else {
            console.error('Something went wrong.');
        }
    }).catch(error => console.error('Unable to add item.', error));
  
    //console.log(actionType, newOrderData); 
}

