
<%@ Page Title="Store Managment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs"  Inherits="WebApplication18.Views.Pages.Store" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h3> <%: Title %>
    </h3>
    
    Store id: <font style="color:red"><b> <%: ViewData["storeId"] %> </b></font><br><br>
    <div class="container">
	    <div id="storeProducts">
        </div>
	</div>

    <script type="text/javascript">
        $(document).ready(function () {
            
            var storeId =<%=ViewData["storeId"]%>;
            var doc = document.getElementById('storeProducts');
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            //////////////////////////////////////
         
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/getStoreProducts?storeId=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                   
                    if (response !== "fail") {
                         var responsJ = JSON.parse(response);
                        var HTML = "";
                        for (i = 0; i < responsJ.length; i++) {

                            var productID = responsJ[i].productID;
                            var productName = responsJ[i].productName;
                            var price = responsJ[i].price;
                            var rank = responsJ[i].rank;
                            var quantityLeft = responsJ[i].quantityLeft;
                            var category = responsJ[i].productCategory;
                            var storeID = responsJ[i].storeID;
                            
                            HTML +=
                                `<div class="col-lg-4 col-md-3 col-sm-6 col-xs-12" style="border: 0.5px solid #e6e6e6; margin-left: -1px; margin-bottom: -1px">
		                        <br>
                                <div class="my-list">
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-6 text-md-left">
			                                <img src=\"../Images/NoImageAvailabe.jpg\" height=\"120\" />         
                                        </div>
                                        <div class="col-12 text-sm-center col-sm-12 col-md-6">
                                            </br><div class="row">
                                                <input type = "button" class="btn btn-primary"  value = "Edit Discount" onclick="discount(` + productID + `);">
                                            </div></br>
                                            <div class="row">
                                                <input type = "button" class="btn btn-danger" value = "Delete Product" onclick="deleteProduct(` + storeID + `,` + productID + `);">
                                            </div>
                                        </div>
                                    </div><br/>
                                    <div class="detail">
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Product id:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <font style="color: red"><b>` + productID + `</b></font>
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Name:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text"   id="inName`+ i + `" value="` + productName + `">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Category:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text" id="inCat`+ i + `" value="` + category + `">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Price:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="15" id="inPrice`+ i + `" name="inPrice" value="` + price + `">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Rank:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="2" id="inRank`+ i + `" value="` + rank + `">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Quantity Left:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="2" id="inQ`+ i + `" value="` + quantityLeft + `">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                <input type = "button" value = "Submit product details" class="btn btn-success" onclick="edit(` + storeID + `,` + productID + `,` + i + `);"> 
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-5">
                                                
                                            </div>
                                        </div><br/> 
		                            </div>
                                </div>
                            </div>`;
                        }
                        HTML +=
                                `<div class="col-lg-4 col-md-3 col-sm-6 col-xs-12" style="border: 0.5px solid #e6e6e6; margin-left: -1px; margin-bottom: -1px">
		                        <br>
                                <div class="my-list">
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-8 text-md-left">
                                            <br/>
                                            <font style="font-size:20px"><b>Add a new one!</b></font><br/>
                                            The product will get an automatically unique ID number.                
                                        </div>
                                        <div>
                                            <img src=\"../Images/new-corner.png\" height=\"120\" />
                                        </div>
                                    </div><br/>
                                    <div class="detail">
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Product id:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <font style="color: red"><b>-----</b></font>
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Name:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text" id="productName">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Category:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text" id="productCategory">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Price:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="15" id="price">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Rank:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="2" id="rank">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Quantity Left:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="number" min="0" size="2" id="quantityLeft">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                <input type="button" value="Add product" id="btnAdd" class="btn btn-danger" onclick="add();"> 
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-5">
                                                
                                            </div>
                                        </div><br/> 
		                            </div>
                                </div>
                            </div>`;
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
        function deleteProduct(storeID, productID, i) {

            var r = confirm("Are you sure you want to delete product " + productID + "?");
            if (r == false) { }
            else {
                event.preventDefault();
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/deleteProduct?storeID=" + storeID + "&productID=" + productID,
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
        } 


        function edit(storeID, productID, i) {
            var r = confirm("Are you sure you edit the details of product " + productID + "?");
            if (r == false) { }
            else {

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
                            alert("Successfuly Updated details");
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
        }
        function discount(productID) {
            console.log("productID " + productID);
            event.preventDefault();
            var getUrl = window.location;
             var baseUrl = getUrl.protocol + "//" + getUrl.host;
             window.location.href = baseUrl + "/Discounts?productID="+productID;
        }  

        function add() {
            event.preventDefault();
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeID =<%=ViewData["storeId"]%>;
            productName = $("#productName").val();
            productCategory = $("#productCategory").val();
            price = $("#price").val();
            rank = $("#rank").val();
            quantityLeft = $("#quantityLeft").val();

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/addProduct?productName=" + productName + "&productCategory=" + productCategory + "&price=" + price + "&rank=" + rank + "&quantityLeft=" + quantityLeft + "&storeID=" + storeID,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response == "ok") {
                        alert("Product added successfuly")
                        location.reload();
                    }
                    else {
                        alert(response);
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
        
    </script>
    
 </asp:Content>


