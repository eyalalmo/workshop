<%@ Page Title="Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewStore.aspx.cs" Inherits="WebApplication18.Views.Pages.ViewStore" %>


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
                   
                    
                    if (respons !== "fail") {
                       var responsJ = JSON.parse(response);

                        var HTML = "";
                        for (i = 0; i < responsJ.length; i++) {

                            var productID = responsJ[i].productID;
                            var productName = responsJ[i].productName;
                            var price = responsJ[i].price;
                            var rank = responsJ[i].rank;
                            var quantityLeft = responsJ[i].quantityLeft;
                            var discount = responsJ[i].discount;
                            var storeID = responsJ[i].storeID;
                           // var storeDiscountMax = responsJ[i].maxPurchasePolicy;
                           // var storeDiscountMin = responsJ[i].minPurchasePolicy;
                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h3>Product Name :  `+ productName + `</h3 >
                            <h3>Product id :  `+ productID + `</h3 >
		                    <div class="detail">
			                    <p></p>`
                                + `price: ` + price + `<br>`
                                + `rank: `+ rank +`<br>`
                                + `quantity Left: `+ quantityLeft + `<br>`
                                + `</div>
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

    </script>
    
 </asp:Content>


