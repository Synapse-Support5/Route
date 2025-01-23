<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeatReailgnment.aspx.cs" Inherits="Route.BeatReailgnment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Beat Reailgnment</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }

            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Style for the file upload container */
        .file-upload-container {
            position: relative;
        }

        /* Style for the file input button */
        .file-upload-input {
            padding-right: 50px; /* Space for the image */
        }

        /* Style for the link that contains the image */
        .file-upload-link {
            position: absolute;
            top: 50%;
            right: 0px; /* Adjust position of the icon */
            transform: translateY(-50%);
            background-color: #e6e6e6; /* Same as the button color */
            border-radius: 3px;
            padding: 5px;
            display: inline-block;
        }

        /* Style for the Excel image */
        .file-upload-icon {
            width: 25px; /* Adjust the size of the image */
            height: 25px;
            background-color: transparent; /* Keep the background transparent */
        }

        /* Container for the progress bar */
        .progress-bar-container {
            width: 100%; /* Makes the container take up the full width */
            /*max-width: 600px;*/ /* Optional: sets a maximum width for larger screens */
            background-color: #e0e0e0;
            border-radius: 0px;
            overflow: hidden;
            position: relative;
            margin-top: -20px; /* Adjusts spacing from the <hr /> */
            margin-bottom: 20px; /* Optional: adds some space after the progress bar */
        }

        /* The animated progress bar */
        .progress-bar {
            height: 5px;
            background: linear-gradient(to right, #4caf50, #81c784, #4caf50); /* Green gradient */
            width: 0%; /* Starts from 0% width */
            animation: progress-animation 2s infinite; /* Animation for progress effect */
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Style the autocomplete dropdown to look like a DropDownList */
        .ui-autocomplete {
            list-style: none;
            margin: 0;
            padding: 0;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            max-height: 200px;
            overflow-y: auto;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            z-index: 1050;
            width: 50px; /* Match the width of the input field */
        }

        .ui-menu-item {
            padding: 8px 12px;
            font-size: 14px;
        }

            .ui-menu-item:hover {
                background-color: #007bff;
                color: white;
            }

        /* Hide any default message shown by jQuery UI */
        .ui-helper-hidden-accessible {
            display: none;
        }

        /*modal stylings*/
        .modal-header {
            display: flex;
            align-items: center;
            gap: 10px; /* Adds space between items */
        }

            .modal-header .form-control {
                margin: 0; /* Reset any extra margin */
            }

            .modal-header input.form-control {
                width: 50%; /* Search box width */
            }

            .modal-header .form-control.d-flex {
                width: 30%; /* Radio button div width */
            }

            .modal-header .btn {
                width: 20%; /* Fetch button width */
            }

        .radio-option input[type="radio"] {
            /*vertical-align: middle;*/ /* Aligns the radio button with text */
            margin-right: 2px; /* Adds space between button and text */
        }

        .fetch-btn {
            width: 20% !important;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a id="HomeLink" class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                        </li>
                        <li class="nav-item">
                            <a id="CreateLink" class="nav-link" runat="server" href="~/Create" onclick="showLoading()">Create</a>
                        </li>
                        <li class="nav-item">
                            <a id="ModifyLink" class="nav-link" runat="server" href="~/Map" onclick="showLoading()">Map</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Transfer" onclick="showLoading()">Transfer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/NewGeo" onclick="showLoading()">NewGeo</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/BeatReailgnment" onclick="showLoading()">BeatReailgnment</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <hr />
        <%--progress bar--%>
        <div id="loadingOverlay" style="display: none; z-index: 9999;">
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        </div>

        <div class="container body-content">
            <div class="headtag">
                <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
            </div>
            <table style="width: 100%; font-family: Calibri; font-size: small">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lbl_msg" Text="" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large" BackColor="LightPink"></asp:Label>
                    </td>
                </tr>
            </table>
            <h2 style="text-align: center; margin-top: 20px;">Beat Reailgnment</h2>
            <br />

            <div class="container">
                <div class="row">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <button type="button" class="form-control" id="btnOpenModal" runat="server" data-toggle="modal" data-target="#exampleModalCenter">
                            Search...
                        </button>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="DistDrp" runat="server" AutoPostBack="true" class="form-control" Style="display: none;" OnSelectedIndexChanged="DistDrp_SelectedIndexChanged">
                        </asp:DropDownList>
                        <input type="text" id="DistSearch" runat="server" class="form-control" placeholder="Enter Distributor" />
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="FromRouteDrp" runat="server" AutoPostBack="true" class="form-control" Style="display: none;" OnSelectedIndexChanged="FromRouteDrp_SelectedIndexChanged">
                        </asp:DropDownList>
                        <input type="text" id="FromRouteSearch" runat="server" class="form-control" placeholder="Enter From Route" />
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="ToRouteDrp" runat="server" AutoPostBack="true" class="form-control" Style="display: none;" OnSelectedIndexChanged="ToRouteDrp_SelectedIndexChanged">
                        </asp:DropDownList>
                        <input type="text" id="ToRouteSearch" runat="server" class="form-control" placeholder="Enter To Route" />
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="Submit" runat="server" Text="Submit" CssClass="btn btn-success form-control" OnClientClick="showLoading()" OnClick="Submit_Click" />
                    </div>
                </div>
            </div>

            <%-- GridView --%>
            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="DistCodeLoadGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;">
                            <Columns>
                                <asp:BoundField DataField="Distcode" HeaderText="Dist. Code" />
                                <asp:BoundField DataField="RouteCode" HeaderText="Route Code" />
                                <asp:BoundField DataField="RouteName" HeaderText="Route Name" />
                                <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                <asp:BoundField DataField="RtrNm" HeaderText="Retailer Name" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="FromRouteLoadGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="Dist. Code" />
                                <asp:BoundField DataField="DistNm" HeaderText="Distributor" />
                                <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                <asp:BoundField DataField="RtrNm" HeaderText="Retailer Name" />
                                <asp:BoundField DataField="UrCode" HeaderText="UrCode" />

                                <%--<asp:TemplateField HeaderText="Select All">
                                    <HeaderTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="parentCheckbox" style="margin-left: -3px;" class="form-check-input" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="CheckBox1" runat="server" class="child-checkbox form-check-input rowCheckbox" style="margin-left: -3px;" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="ToRouteLoadGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="Dist. Code" />
                                <asp:BoundField DataField="DistNm" HeaderText="Distributor" />
                                <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                <asp:BoundField DataField="RtrNm" HeaderText="Retailer Name" />
                                <asp:BoundField DataField="UrCode" HeaderText="UrCode" />

                                <asp:TemplateField HeaderText="Select All">
                                    <HeaderTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="parentCheckbox" style="margin-left: -3px;" class="form-check-input" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="CheckBox1" runat="server" class="child-checkbox form-check-input rowCheckbox" style="margin-left: -3px;" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <%-- Modal for Search button --%>
            <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header d-flex align-items-center">
                            <asp:TextBox ID="SearchTxt" runat="server" placeholder="Search..." CssClass="form-control mr-3" Style="width: 70%;" />

                            <div class="form-control d-flex align-items-center justify-content-center mr-3" style="width: 30%;">
                                <asp:RadioButton ID="rbActive" runat="server" GroupName="Status" Text="Active" CssClass="radio-option mr-3" Style="margin-top: 8px;" />
                                <asp:RadioButton ID="rbInactive" runat="server" GroupName="Status" Text="InActive" CssClass="radio-option" Style="margin-top: 8px;" />
                            </div>
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanelFetch" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnFetch" runat="server" CssClass="btn btn-primary" Text="      Fetch      " OnClick="btnFetch_Click" Style="width: 100%;" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>


                        <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="SearchModalGrid" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                            AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                            <Columns>
                                                <asp:BoundField DataField="Distcode" HeaderText="Dist. Code" />
                                                <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                                <asp:BoundField DataField="RtrCode" HeaderText="Rtr Code" />
                                                <asp:BoundField DataField="UrCode" HeaderText="UrCode" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />

                                            </Columns>
                                            <HeaderStyle CssClass="header-hidden" />
                                            <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <%--<button type="button" class="btn btn-primary" onclick="selectItems()" data-dismiss="modal">Select</button>--%>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>

    </form>


    <%--script for progressbar--%>
    <script>
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'block';
        }

        window.onload = function () {
            document.getElementById('loadingOverlay').style.display = 'none';
        };
    </script>

    <%--Script for Dropdown Auto Search--%>
    <script>
        $(document).ready(function () {
            // Get DropDownList options and convert to an array
            var options = [];
            $('#<%= DistDrp.ClientID %> option').each(function () {
                var text = $(this).text();  // DistNm
                var value = $(this).val();  // DistCode
                if (value) {
                    options.push({ label: text, value: value });
                }
            });

            // Initialize jQuery UI Autocomplete
            $('#DistSearch').autocomplete({
                source: options,
                select: function (event, ui) {
                    // Set the selected value in the input box (DistNm)
                    $('#DistSearch').val(ui.item.label);

                    // Update the hidden DropDownList (DistDrp) value
                    $('#<%= DistDrp.ClientID %>').val(ui.item.value);

                    // Trigger OnSelectedIndexChanged event of DropDownList
                    __doPostBack('<%= DistDrp.UniqueID %>', '');

                    showLoading();

                    // Return true to avoid clearing the selected value
                    return true; // Ensures value remains in the input field
                }
            });
        });

        $(document).ready(function () {
            // Get DropDownList options and convert to an array
            var options = [];
            $('#<%= FromRouteDrp.ClientID %> option').each(function () {
                var text = $(this).text();  // RouteName
                var value = $(this).val();  // RouteId
                if (value) {
                    options.push({ label: text, value: value });
                }
            });

            // Initialize jQuery UI Autocomplete
            $('#FromRouteSearch').autocomplete({
                source: options,
                select: function (event, ui) {
                    // Set the selected value in the input box (RouteName)
                    $('#FromRouteSearch').val(ui.item.label);

                    // Update the hidden DropDownList (FromRouteDrp) value
                    $('#<%= FromRouteDrp.ClientID %>').val(ui.item.value);

                    // Trigger OnSelectedIndexChanged event of DropDownList
                    __doPostBack('<%= FromRouteDrp.UniqueID %>', '');

                    showLoading();

                    // Return true to avoid clearing the selected value
                    return true; // Ensures value remains in the input field
                }
            });
        });

        $(document).ready(function () {
            // Get DropDownList options and convert to an array
            var options = [];
            $('#<%= ToRouteDrp.ClientID %> option').each(function () {
                var text = $(this).text();  // RouteName
                var value = $(this).val();  // RouteId
                if (value) {
                    options.push({ label: text, value: value });
                }
            });

            // Initialize jQuery UI Autocomplete
            $('#ToRouteSearch').autocomplete({
                source: options,
                select: function (event, ui) {
                    // Set the selected value in the input box (RouteName)
                    $('#ToRouteSearch').val(ui.item.label);

                    // Update the hidden DropDownList (ToRouteDrp) value
                    $('#<%= ToRouteDrp.ClientID %>').val(ui.item.value);

                    // Trigger OnSelectedIndexChanged event of DropDownList
                    __doPostBack('<%= ToRouteDrp.UniqueID %>', '');

                    showLoading();

                    // Return true to avoid clearing the selected value
                    return true; // Ensures value remains in the input field
                }
            });
        });
    </script>

    <%-- Script for selectall checkboxes in RouteTransferSplitGridview --%>
    <script>
        $(document).ready(function () {
            const updateParentCheckbox = () => {
                const childCheckboxes = $(".child-checkbox");
                const checkedCheckboxes = childCheckboxes.filter(":checked");
                const parentCheckbox = $("#parentCheckbox");

                if (checkedCheckboxes.length === 0) {
                    parentCheckbox.prop("checked", false).prop("indeterminate", false);
                } else if (checkedCheckboxes.length === childCheckboxes.length) {
                    parentCheckbox.prop("checked", true).prop("indeterminate", false);
                } else {
                    parentCheckbox.prop("checked", false).prop("indeterminate", true);
                }
            };

            $("#parentCheckbox").on("change", function () {
                const isChecked = $(this).is(":checked");
                $(".child-checkbox").prop("checked", isChecked);
            });

            $(".child-checkbox").on("change", updateParentCheckbox);

            updateParentCheckbox(); // Initialize state on page load
        });
</script>

</body>
</html>
