<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="WebApplication18.Views.Pages.ManageStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron d-flex align-items-center">
  <div class="container">
    <h1><%: Title +"Store Staff Panel" %></h1>   
    <h1> </h1>
      <h1> </h1>
      <div class="container ">
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
              <div class="clearfix">
                  <div class="clearfix">
    <button type="button"  name="addManager" id="addManager" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Add Manager</button>
                  </div>
                  </div>
              </div>
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
              <h3>Add an Owner</h3>
              <input class="form-control align-middle" type="text"  visible="false" placeholder="Username"  id="owner" name="owner"/>
        <button type="button" class="btn btn-primary" onClick="confirmOwner()" id="confirm" name="confirm"/>
             Confirm
              </div>
          </div>
       </div>
    
    <div class="container">
	<div id="allRoles" class="row">
     <h2>Staff:</h2>
	</div>
	</div>
    
         
  </div>
    <div class="container">
	<div id="allPending" class="row">
	</div>
	</div>
 
<div class="modal" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">

     
      <div class="modal-header">
        <h2 class="modal-title">Please Enter Username</h2>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>

      <!-- Modal body -->
      <div class="modal-body">
        <input class="form-control" type="text" placeholder="Manager Username" aria-label="manager" id="manager" name="manager">
          <h3>Permissions:</h3>
          <div class="checkbox">
  <label><input type="checkbox" id="per1" checked="">Edit Products</label>
</div>
<div class="checkbox">
  <label><input type="checkbox" id="per2" checked="">Edit Discounts</label>
</div>
<div class="checkbox">
  <label><input type="checkbox" id="per3" checked="">Edit Policy</label>
</div>

      </div>

      <!-- Modal footer -->
      <div class="modal-footer">
         <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" onClick="addM()" id="addman" >Add Manager</button>
      </div>

    </div>
  </div>
</div>
    <script type="text/javascript">
 
        function getRoles(response) {
            var doc = document.getElementById('allRoles');
            var i;
            var jsonList = JSON.parse(response);
            var HTML="";
            for (i = 0; i < jsonList.length; i++) {
                var appointed = ""
                var buttons = "";
                var role = "Store Owner"
                    if (jsonList[i].isOwner===false) {
                        role = "Store Manager"
                        appointed = "Appointed By: " + jsonList[i].appointedBy.username;
                        buttons = `<button type="button" class="btn btn-primary" onClick="removeRole( \'`+ jsonList[i].user.username +`\' )"/>
             Remove
            </button>`
                    }
                HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			<h4>`+ role + `\n</h4>
            <h5> Username:`+ jsonList[i].user.username + `\n</h5>
			<span> ` + appointed + `\n</span>
            <div class="clearfix">
            `+buttons+`
        </div>
		</div>
		</div>`
        
            }
            doc.innerHTML += HTML;

            
        };

        function getPending(response) {
            var doc = document.getElementById('allPending');
            var i;
            console.log(response);
            var jsonList = JSON.parse(response);
            var HTML = "";
            
            if (jsonList.length > 0) {
                HTML+=`<h2>Pending Store Owners:</h2>`
            }
            for (i = 0; i < jsonList.length; i++) {
                HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
            <h5> Username:`+ jsonList[i] + `\n</h5>
            <div class="clearfix">
            <button type="button" class="btn btn-primary" onClick="signContract( \'`+ jsonList[i] +`\' )"/>
             Sign Contract
            </button>
            <button type="button" class="btn btn-danger" onClick="declineContract( \'`+ jsonList[i] +`\' )"/>
             declineContract
            </button>
        </div>
		</div>
		</div>`
        
            }
            doc.innerHTML += HTML;

            
        };
        function addM() {
                    var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;
            var username = $("#manager").val();
            var prod = $("#per1").is(':checked'); 
            var disc = $("#per2").is(':checked'); 
            var poli = $("#per3").is(':checked'); 

        
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/addManager?username=" + username + "&storeID=" + storeId + "&prod=" + prod + "&disc=" + disc + "&poli=" + poli,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Manager successfuly  added")
                     window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        };
    </script>
    <script type="text/javascript">
        function removeRole(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/removeRole?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Staff member successfuly  removed")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function signContract(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/signContract?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Contract successfuly signed")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function declineContract(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/declineContract?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Declined the contract. The user will not be appointed as an owner")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
        function confirmOwner() {
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var username = $("#owner").val();
            var storeId =<%=ViewData["storeId"]%>;
                
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/addOwner?username=" + username +"&storeID=" + storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response !== "ok") {
                            alert(response);
                        } else
                            alert("User has been successfuly offered as a store owner. Contract requests sent to all the other relevant owners")
                            window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
            });
            
            };
        

        
    </script>
<script type="text/javascript">

    $(document).ready(function () {
            
                  var storeId =<%=ViewData["storeId"]%>;
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/getAllRoles?storeId="+ storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        
                        getRoles(response);
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
        });
         jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/getAllPending?storeId="+ storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        getPending(response);
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
        });



           
     });

    </script>
    </asp:Content>