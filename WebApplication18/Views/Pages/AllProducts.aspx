﻿<%@ Page Title="Products Catalog" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllProducts.aspx.cs" Inherits="WebApplication18.Views.Pages.AllProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h3><span class="glyphicon glyphicon-barcode"></span>  <%: Title %></h3>
  <div class="container">

      <div class="container">

          <button name="showAll" id="showAll" class="btn btn-primary">Show All Products</button> &nbsp &nbsp &nbsp                 
            <p></p><p></p>
     
          <input class="form-control" type="text" placeholder="Search" aria-label="Search" id="searchI" name="searchI">
          <p></p><p></p>
          <button  name="searchN" id="searchN" class="btn btn-primary">Search By Name</button> &nbsp &nbsp &nbsp
          <button  name="searchC" id="searchC" class="btn btn-primary">Search By Category</button>&nbsp &nbsp &nbsp
          <button  name="searchC" id="searchK" class="btn btn-primary">Search By Keyword</button>

          <p></p><p></p> 
          <h3> Filters  <button class="btn btn-primary" id="clearFilters">clear</button> </h3>
          <p></p><p></p>
          Min price: <input type="number" aria-label="Minimum Price" id="minPrice" name="minPrice" min="0" max="1000000" value="0">&nbsp &nbsp &nbsp
          Max price: <input type="number" aria-label="Maximum Price" id="maxPrice" name="maxPrice" min="0" max="1000000" value="1000000"> <p></p> <p></p>
          Min rank:  <input type="number" id="minRank" min="0" max="5" value="0" step="0.1">
         
      </div>
  </div>
    <div class="container">
        <p></p><p></p>
	<div id="allProducts" class="row">
        </div>
        <p></p><p></p>
		</div>
       <%--  <input class="form-control align-middle" type="text"  visible="false" placeholder="Quantity" aria-label="Search" id="quantity" name="quantity"/>
        <button type="button" class="btn btn-primary" onClick="confirmBasket()" id="confirm" name="confirm"/>
             Confirm--%>

 
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
    <script type="text/javascript">
        function getProducts(response,baseUrl) {
            var doc = document.getElementById('allProducts');
            doc.innerHTML = "";
            var i;
            var jsonList = JSON.parse(response);
            var HTML = "<div class=\"card-body\"><div class=\"row\style=\"background-color:black\"> </div></div>";
            var minPrice = document.getElementById("minPrice").value;
            var maxPrice = document.getElementById("maxPrice").value;
            var rank = document.getElementById("minRank").value;
            for (i = 0; i < jsonList.length; i++) {

                            HTML +=
                                `<hr><div class="card-body">
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-2 text-center">
                                            <img src="../Images/NoImageAvailabe.jpg" height="80" />
                                        </div>
                                        <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                            <h4 class="product-name">
                                                <strong>` + productName + `</strong>
                                            </h4>
                                        </div> 
                                        <div class="col-12 col-sm-12 text-sm-center col-md-4 text-md-right row"> 
                                            <div class="col-2 col-sm-2 col-md-4 text-md-right" style="padding-top: 5px">  
                                                <h5>
                                                    <strong>$" + price + "<span class="text-muted"> x</span></strong>
                                                </h5>
                                            </div>
                                            <div class="quantity"><input type="button" value="+" onclick="plusQuantity(` + id + `,` + quantity + `);" class="plus">
                                                <input type="text" value="` + quantity + `" title="Qty" class="qty">
                                                <input type="button" value="-" onclick="minusQuantity(` + id + `,` + quantity + `);" class="minus">
                                            </div>
                                            <div class="col-2 col-sm-2 col-md-2 text-right">
                                                <button type="button" class="btn btn-danger" onclick="deleteRow(` + id + `);">
                                                <i class="fa fa-trash" aria-hidden="true"></i></button>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;
            

                /*if (jsonList[i].price >= minPrice && jsonList[i].price <= maxPrice && jsonList[i].rank >= rank){
                HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12" id="` + jsonList[i].productID + `">
		                   <div class="my-list">
            			<p><b>`+ jsonList[i].productName + `</b></p>
                        <span> Category: `+ jsonList[i].productCategory + `</span> <p></p>      
            			<span > Quantity Left: `+ jsonList[i].quantityLeft + `</span> <p></p>   
            			<div class="detail">
	            		<span> Price: `+ jsonList[i].price + `$</span></p> <p></p>
	            		<span> Rank: `+ jsonList[i].rank + `</span></p> <p></p>
            		    <button type="button" class="btn btn-primary" onClick="addToCart(` + jsonList[i].productID + `)"/>
                       Add To Cart
                          </button>`
           + "<a href=\"" + baseUrl + "/ViewStore?storeId=" + jsonList[i].storeID + "\" class=\"btn btn-info\">Go To Store</a>" +
			`</div>
		</div>
		</div>`*/
                //`+jsonList[i].ProductID+`
                }
            }
            doc.innerHTML = HTML;
        };
    </script>
    <script type="text/javascript">
        var currentProductID;
        function addToCart(productID) {
            currentProductID = productID;
            $("#enterQuantity").modal()
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
                            location.reload();
                    },
                    error: function (response) {
                        console.log(response);
                    }
            });
              
            
        }
        
    </script>
<script type="text/javascript">

    $(document).ready(function () {
            $("#clearFilters").click(function clearFilters() {
                document.getElementById('minPrice').value = 0; 
                document.getElementById('maxPrice').value = 1000000;
                document.getElementById('minRank').value = 0;
            });
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
                        getProducts(response,baseUrl);
                        
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
                        getProducts(response,baseUrl);
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
                       getProducts(response,baseUrl);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>
    </asp:Content>