    <%@ Page  Title="Edit Stores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.EditStore" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h3><%: Title %>
    </h3>
     
    <style>

        button{
          background:#0099ff;
          color:#fff;
          border:none;
          position:relative;
          height:60px;
          width:120px;
          font-size:1.0em;
          padding:0 2em;
          cursor:pointer;
          transition:800ms ease all;
          outline:none;
        }
        button:hover{
          background:#fff;
          color:#0099ff;
        }
        button:before,button:after{
          content:'';
          position:absolute;
          top:0;
          right:0;
          height:2px;
          width:0;
          background: #0099ff;
          transition:400ms ease all;
        }
        button:after{
          right:inherit;
          top:inherit;
          left:0;
          bottom:0;
        }
        button:hover:before,button:hover:after{
          width:100%;
          transition:800ms ease all;
        }

    </style>

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
         var getUrl = window.location;
          var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
        $(document).ready(function () {
            var doc = document.getElementById('allStores');
        
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/user/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "fail") {
                      var responsJ= JSON.parse(response);
                        var HTML = "";
                        for (i = 0; i < responsJ.length; i++) {    
                            
                            var storeId = responsJ[i].storeId;
                            var storeName = responsJ[i].name;
                            var description = responsJ[i].description;
                            var active = responsJ[i].active;
                            var activeScript;
                            if(active)
                                activeScript = `<font style="color:green; font-size=12px;"> <b>Active!</b></font>`
                            else 
                                activeScript = `<font style="color:red; font-size=12px;"> <b>Close</b></font>`
                            HTML +=
                                `<div class="col-lg-3 col-md-4 col-sm-6 col-xs-12" style="border: 0.5px solid #e6e6e6; margin-left: -1px; margin-bottom: -1px">
		                        <br>
                                <div class="my-list">
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-8 text-md-left">
			                                <font style="font-size:20px"><b>` + storeName + `</b></font>
                                            (id: <font style="color: red"><b>` + storeId + `</b></font>) 
                                        </div>
                                        <div class="col-12 col-sm-12 col-md-3 text-md-left">
			                                ` + activeScript + `
                                        </div>
                                    </div><br/>
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-12 text-md-left">
			                                ` + description + `      
                                        </div>
                                    </div><br/>
                                    <div class="detail">
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-6 text-md-left">` +
                                                 "<button onclick=\"manageProducts(" + storeId + ");\">Products</<button>"          
                                            +`</div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">`+
                                                "<button onclick=\"editStoreDiscount(" + storeId + ");\">Discounts</<button>" 
                                            +`</div>
                                        </div><br/>
                                        <div class="row">
                                            <div class="col-12 col-sm-12 col-md-6 text-md-left">`+
                                                "<button onclick=\"manageStaff(" + storeId + ");\">Staff</<button>" 
                                            +`</div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">`+
                                                "<button onclick=\"manageStorePolicy(" + storeId + ");\">Policy</<button>" 
                                            +`</div>
                                        </div><br/>
                                    </div>
                                </div>
                            </div>`;
                        }

                        HTML +=
                                `<div class="col-lg-3 col-md-4 col-sm-6 col-xs-12" style="border: 0.5px solid #e6e6e6; margin-left: -1px; margin-bottom: -1px">
		                        <br>
                                <div class="my-list">
                                    <div class="row"> 
                                        <div class="col-12 col-sm-12 col-md-9 text-md-left">
			                                <font style="font-size:16px"><b>Open a new one!</b></font><br/>
                                            The store will get an automatically unique ID number.         
                                        </div>
                                        <div>
			                                <img src=\"../Images/new-corner.png\" height=\"60\" />      
                                        </div>
                                    </div><br/>
                                    <div class="detail">
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Name:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text" id="name" size="15">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                Description:         
                                            </div>
                                            <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                                                <input type="text" id="des" size="15">
                                            </div>
                                        </div><br/>
                                        <div class="row"> 
                                            <div class="col-12 col-sm-12 col-md-4 text-md-left">
                                                <input type="button" value="Open store" id="btnAdd" class="btn btn-danger" onclick="add();"> 
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


        function add() {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
                
                name = $("#name").val();
                des = $("#des").val();
                 
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addStore?name=" + name + "&description=" + des,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response === "ok") {
                            alert("Store added successfuly");  
                            window.location.href = baseUrl + "/EditStore";
                           
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
                        window.location.href = baseUrl + "/StorePolicy?storeID=" + id;
                    }
                    else {
                        alert("you dont have the permissions to edit Policy");
                    }
                },
                error: function (response) {
                   
                    window.location.href = baseUrl + "/Default";
                }
            });

        }
    </script>
 </asp:Content>



