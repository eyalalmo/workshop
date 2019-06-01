    <%@ Page  Title="Edit Stores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.EditStore" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
     

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
         var getUrl = window.location;
          var baseUrl = getUrl.protocol + "//" + getUrl.host
        $(document).ready(function () {
            var doc = document.getElementById('allStores');
           
        
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/user/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    if (response !== "fail") {
                        var responsJ = JSON.parse(response);
                        console.log(response)
                        var HTML = "";
                        for (i = 0; i < responsJ.length; i++) {    
                            
                            var storeId = responsJ[i].storeId;
                            var storeName = responsJ[i].name;
                            var description = responsJ[i].description;
                            var active = responsJ[i].active;

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			                <h2>Store Name:  `+ storeName+`</h2 >
		                    <h4>Store ID:  `+ storeId +`</h4 >
                             <h4>description:  `+ description +`</h4 >
                            <h4>Is Active:  `+ active+`</h4 >

			                    <div class="detail">
			                    <p></p>`
                                + "<form><a href =\"" + baseUrl + "/Store?storeId=" + storeId + "\"  id=\"manageProducts" + i + "\"  onclick=\"manageProducts(" + storeId + ");\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Manage Store Products</a></form>" 
                                 + "<p></p><form><a href=\"" + baseUrl + "/StoreDiscount?storeID=" + storeId + "\"  id=\"editStoreDiscount" + i + "\"  onclick=\"editStoreDiscount(" + storeId + ");\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Edit Store Discounts</a></form>" 
                               + "<p></p><a href=\"" + baseUrl + "/ViewStore?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store Products</a>" 
                                + "<p></p><form><a href=\"" + baseUrl + "/ManageStaff?storeId=" + storeId + "\" id=\"manageStaff" + i + "\" onclick=\"manageStaff(" + storeId + ");\"  class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Staff Managment</a></form>"
                                + "<p></p><form><a href=\"" + baseUrl + "/AddProduct?storeId=" + storeId + "\"   id=\"addProduct" + i + "\"  onclick=\"addProduct(" + storeId + ");\"  class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Add Product</a></form>" 
                                + "<p></p><form><a href=\"" + baseUrl + "/EditStorePolicy?storeId=" + storeId + "\"    id=\"manageStorePolicy" + i + "\"  onclick=\"manageStorePolicy(" + storeId + ");\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >Edit Store Policy</a></form>"
			                 +`   </div>
		                    </div>
		                    </div>`
                        }
                        doc.innerHTML += HTML;
                    }
                    else {
                        alert(responsJ);
                    }
                },
                error: function (response) {
                    //alert("Lost DB connection");
                    window.location.href = baseUrl+"/index";
                }
            });
            

        });
        function manageProducts(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/IsEP?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                         window.location.href = baseUrl + "/Store?storeId=" + id;
                    }
                    else {
                        alert("you dont have the permissions to edit Products");
                    }
                },
                 error: function (response) {
                    
                    window.location.href = baseUrl + "/Default";
                }
            });
          
        }
         function manageStaff(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/isOwner?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                         window.location.href = baseUrl + "/ManageStaff?storeId=" + id;
                    }
                    else {
                        alert("you dont have the permissions to Manage Staff");
                    }
                },
                 error: function (response) {
                    
                    window.location.href = baseUrl + "/Default";
                }
            });
          
            }

        function addProduct(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/IsEP?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                         window.location.href = baseUrl + "/AddProduct?storeId=" + id;
                    }
                    else {
                        alert("you dont have the permissions to edit Products");
                    }
                },
                 error: function (response) {
                    
                    window.location.href = baseUrl + "/Default";
                }
            });
          
            }
        function editStoreDiscount(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/IsED?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                        baseUrl + "/StoreDiscouts?storeId=" + id
                         window.location.href =  baseUrl + "/StoreDiscounts?storeId=" + id
                    }
                    else {
                        alert("you dont have the permissions to edit Discount");
                    }
                },
                 error: function (response) {
                     
                    window.location.href = baseUrl + "/Default";
                }
            });
          
            }
            function manageStorePolicy(id) {
            event.preventDefault();
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/IsEPo?storeId=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
          
                    if (response === true) {
                         window.location.href = baseUrl + "/EditStorePolicy?storeID=" + id;
                    }
                    else {
                        alert("you dont have the permissions to edit Policy");
                    }
                },
                 error: function (response) {
                     alert("dddd")
                    window.location.href = baseUrl + "/Default";
                }
            });
          
            }
    </script>
 </asp:Content>



