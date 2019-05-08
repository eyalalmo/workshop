<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllProducts.aspx.cs" Inherits="WebApplication18.Views.Pages.AllProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron d-flex align-items-center">
  <div class="container">
    <h1>Products Catalog</h1>   
    <h1> </h1>
      <h1> </h1>
      <div class="container ">
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
    <button  name="showAll" id="showAll" class="btn btn-primary">Show All Products</button>
                  </div>
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
              <input class="form-control" type="text" placeholder="Search" aria-label="Search" id="searchI" name="searchI">
    <button  name="searchN" id="searchN" class="btn btn-primary">Search By Name</button>
    <button  name="searchC" id="searchC" class="btn btn-primary">Search By Category</button>
    <button  name="searchC" id="searchK" class="btn btn-primary">Search By Keyword</button>
              </div>
          </div>
       </div>
    
    <div class="container">
	<div id="allProducts" class="row">
        </div>
		</div>
         <input class="form-control align-middle" type="text"  visible="false" placeholder="Quantity" aria-label="Search" id="quantity" name="quantity"/>
        <button type="button" class="btn btn-primary" onClick="confirmBasket()" id="confirm" name="confirm"/>
             Confirm
  </div>
 
<%--<div class="modal" id="enterQuantity">
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
          <input type="hidden" name="bookId" id="bookId" value=""/>
      </div>

      <!-- Modal footer -->
      <div class="modal-footer">
         <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button  name="quant" id="quant" class="btn btn-primary">Add To Cart</button>
      </div>

    </div>
  </div>
</div>--%>
    <script type="text/javascript">
        function getProducts(response) {
            var doc = document.getElementById('allProducts');
            doc.innerHTML = "";
            var i;
            var jsonList = JSON.parse(response);
            var HTML;
            for (i = 0; i < jsonList.length; i++) {
                HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			<span id="hhh">`+ jsonList[i].productName + `</span>
            <span> Category:`+ jsonList[i].productCategory + `</span>
			<span class="pull-right"> Quantity Left:`+ jsonList[i].quantityLeft + `</span>
			<div class="detail">
			<p>Price:`+ jsonList[i].price + `</p>
		    <button type="button" class="btn btn-primary" onClick="addToCart(` + jsonList[i].productID + `)"/>
             Add To Cart
            </button>
            <a href="#" class="btn btn-info">Go To Store</a>
			</div>
		</div>
		</div>`
                //`+jsonList[i].ProductID+`


            }
            doc.innerHTML = HTML;
        };
    </script>
    <script type="text/javascript">
        var currentProductID;
        function addToCart(productID) {
            document.getElementById('quantity').style.visibility = "visible";
            currentProductID = productID;
            document.getElementById('confirm').style.visibility = "visible";
        };
        function confirmBasket() {
             var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
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
              document.getElementById('quantity').style.visibility = "hidden";
              document.getElementById('confirm').style.visibility = "hidden";
            
        }
        

        
    </script>
<script type="text/javascript">

    $(document).ready(function () {
        document.getElementById('quantity').style.visibility = "hidden";
        document.getElementById('confirm').style.visibility = "hidden";
            $("#showAll").click(function () {
                event.preventDefault();
                
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl +"/api/products/getAllProducts",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        getProducts(response);
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });

        $("#searchN").click(function () {
                 document.getElementById('quantity').style.visibility = "hidden";
                 document.getElementById('confirm').style.visibility = "hidden";
                event.preventDefault();
                 search = $("#searchI").val();
                var doc = document.getElementById('allProducts')
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl +"/api/products/searchByName?param="+ search,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        getProducts(response);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        $("#searchC").click(function () {
                 document.getElementById('quantity').style.visibility = "hidden";
                 document.getElementById('confirm').style.visibility = "hidden";
                event.preventDefault();
                 search = $("#searchI").val();
                var doc = document.getElementById('allProducts')
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl +"/api/products/searchByCat?param="+ search,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                       getProducts(response);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        $("#searchK").click(function () {
                 document.getElementById('quantity').style.visibility = "hidden";
                 document.getElementById('confirm').style.visibility = "hidden";
                event.preventDefault();
                 search = $("#searchI").val();
                var doc = document.getElementById('allProducts')
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl +"/api/products/searchByKey?param="+ search,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                       getProducts(response);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>
    </asp:Content>