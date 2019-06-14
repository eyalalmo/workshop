<%@ Page Title="Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewStore.aspx.cs" Inherits="WebApplication18.Views.Pages.ViewStore" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title +" id:  " + ViewData["storeId"] %>
    </h2>
    

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <div class="modal" id="enterQuantity">
  <div class="modal-dialog">
    <div class="modal-content">

      <!-- Modal Header -->
      <div class="modal-header">
        <h2 class="modal-title">Please Enter Quantity</h2>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>

      <!-- Modal body -->
      <div class="modal-body">
        <input class="form-control" type="text" placeholder="Quantity" aria-label="Search" id="quantity" name="quantity">
      </div>

      <!-- Modal footer -->
      <div class="modal-footer">
         <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button type="button" id="confirm" onclick="confirmBasket()" class="btn btn-primary" data-dismiss="modal">Confirm</button>
      </div>

    </div>
  </div>
   </div>

    <script type="text/javascript">
        $(document).ready(function () {

            var storeId =<%=ViewData["storeId"]%>;
            var doc = document.getElementById('allStores');
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/getStoreProducts?storeId=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var HTML = `<p> <button type="button" class="btn btn-primary" onClick="storeDiscounts(`+storeId+`)"/>Store Discounts</button></p>
                              <p><button type="button" class="btn btn-primary" onClick="storePolicies(`+storeId+`)"/>Store Policies</button></p>`;
                    var doc = document.getElementById('allStores');
                
      
                    if (response !== "fail") {
                        var responsJ = JSON.parse(response);
                       
                        for (i = 0; i < responsJ.length; i++) {

                           
                           HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" id="` + responsJ[i].productID + `">
		                   <div class="my-list">
            			<p><b>`+ responsJ[i].productName + `</b></p>
                        <span> Category: `+ responsJ[i].productCategory + `</span> <p></p>      
            			<span > Quantity Left: `+ responsJ[i].quantityLeft + `</span> <p></p>   
            			<div class="detail">
	            		<span> Price: `+ responsJ[i].price + `$</span></p> <p></p>
	            		<span> Rank: `+ responsJ[i].rank + `</span></p> <p></p>
            		    <button type="button" class="btn btn-primary" onClick="addToCart(` + responsJ[i].productID + `)"/>
                       Add To Cart
                          </button>`+

			            `</div>
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
        var currentProductID;
          function addToCart(productID) {
            currentProductID = productID;
            $("#enterQuantity").modal()
        };
        function confirmBasket() {
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
            var amount = $("#quantity").val();
            console.log(baseUrl);
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/products/addToBasket?productID=" + currentProductID + "&amount=" + amount,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Added succesfully to basket")

                },
                error: function (response) {
                    console.log(response);
                }
            });
            
        };
               function storeDiscounts(storeId) {
               event.preventDefault();
               var getUrl = window.location;
                   var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               window.location.href = baseUrl+"/DiscountView?storeId=" + storeId;
        }
               function storePolicies(storeId) {
               event.preventDefault(storeId);
               var getUrl = window.location;
                   var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               window.location.href = baseUrl+"/PolicyView?storeId=" + storeId;
          }
              
    </script>
    
 </asp:Content>


