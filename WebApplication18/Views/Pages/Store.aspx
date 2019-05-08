<%@ Page Title="Store Managment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs"  Inherits="WebApplication18.Views.Pages.Store" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title +" id:  " + ViewData["storeId"] %>
    </h2>
    

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
        $(document).ready(function () {

            var storeId =<%=ViewData["storeId"]%>;
            var doc = document.getElementById('allStores');
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/getStoreProducts?storeId=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var responsJ = JSON.parse(response);
                    console.log(response);
                    if (responsJ !== "fail") {

                        var HTML = "";
                        for (i = 0; i < responsJ.length; i++) {

                            var productID = responsJ[i].productID;
                            var productName = responsJ[i].productName;
                            var price = responsJ[i].price;
                            var rank = responsJ[i].rank;
                            var quantityLeft = responsJ[i].quantityLeft;
                            var discount = responsJ[i].discount;
                            var storeID = responsJ[i].storeID;

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h3>Product Name :  `+ productName + `</h3 >
                            <h3>Product id :  `+ productID + `</h3 >
		                    <div class="detail">
			                    <p></p>`
                                + `productName: <input type="text" id="inName`+i+`" value="` + productName + `"><br>`
                                + `price: <input type="text" id="inPrice`+i+`" name="inPrice" value="` + price + `"><br>`
                                + `rank: <input type="text" id="inRank`+i+`" value="` + rank + `"><br>`
                                + `quantityLeft: <input type="text" id="inQ`+i+`" value="` + quantityLeft + `"><br>`
                                + "<form><input type = \"button\" value = \"Discount\" onclick=\"discount(" + storeID + "," + productID +");\"></form>"
                                + " <form><input type = \"button\" value = \"edit\" onclick=\"edit(" + storeID + "," + productID + "," +i +");\"></form>"
                                + `   </div>
		                    </div>
		                    </div>`


                        }
                        doc.innerHTML += HTML;

                    }
                    else {
                        alert("problem");
                    }
                },
                error: function (response) {
                  
                    window.location.href = baseUrl + "/Default";
                }
            });

          
        });
         function edit (storeID, productID,i) {
            event.preventDefault();
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            //console.log(price + "  " + rank + "  " + quantityLeft)
             var price = document.getElementById("inPrice" + i).value;
             var rank = document.getElementById("inRank" + i).value;
             var quantityLeft = document.getElementById("inQ" + i).value;
             var productName = document.getElementById("inName" + i).value;
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/SetProductInformation?storeID=" + storeID + "&productID=" + productID + "&price=" + price + "&rank=" + rank + "&quantityLeft=" + quantityLeft + "&productName=" + productName,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response === "ok") {
                        alert("sucsses!");
                        window.location.href = baseUrl + "/Store?storeId=" + storeID;
                    }
                    else {
                        alert(response);
                    }
                },
                error: function (response) {
                    //alert("Lost DB connection");
                    window.location.href = baseUrl + "/index";
                }
            });

        }  
        function discount(productID) {
            event.preventDefault();
            var getUrl = window.location;
             var baseUrl = getUrl.protocol + "//" + getUrl.host;
             window.location.href = baseUrl + "/Discounts?productID="+productID;
        }  

    </script>
    
 </asp:Content>


