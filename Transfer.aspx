<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="Route.Transfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transfer</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />

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
            position: relative;
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
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
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

        function showNoteAlert2() {
            $('#noteAlert2').show();
        }

        function hideNoteAlert2() {
            $('#noteAlert2').hide();
        }
    </script>

    <%--script for scroll to grid from view button--%>
    <script type="text/javascript">
        function scrollToGrid(isVisible) {
            var grid = document.getElementById('<%= ToDistExistViewGrid.ClientID %>');
            if (grid) {
                if (isVisible) {
                    grid.scrollIntoView({ behavior: 'smooth', block: 'start' });
                } else {
                    // Scroll back to the top (or another element) when hiding the grid
                    window.scrollTo({ top: 0, behavior: 'smooth' });
                }
            }
        }
    </script>

    <%--script to get all toasts at a time on Transfer button click--%>
    <script type="text/javascript">
        function showMessagesWithDelay(messages) {
            let delay = 2000; // 2 seconds

            messages.forEach((msg, index) => {
                setTimeout(() => {
                    showToast(msg, "toast-success");
                }, index * delay);
            });
        }
    </script>

</head>
<body>
    <form id="form2" runat="server">

        <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Create" onclick="showLoading()">Create</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Map" onclick="showLoading()">Map</a>
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
            <h2 style="text-align: center; margin-top: 20px;">Transfer Route
                <%--<asp:Label runat="server" ID="LabelId" Text=""></asp:Label>--%>
            </h2>
            <br />

            <div id="routeTransferDiv" runat="server" class="container mt-3">
                <div class="row mb-3">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="StateDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="StateDrp_SelectedIndexChanged">
                            <asp:ListItem Text="State" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="AreaDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="AreaDrp_SelectedIndexChanged">
                            <asp:ListItem Text="Area" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="ZoneDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="ZoneDrp_SelectedIndexChanged">
                            <asp:ListItem Text="Zone" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="FromDistDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="FromDistDrp_SelectedIndexChanged">
                            <asp:ListItem Text="From Distributor" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="TypeDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="TypeDrp_SelectedIndexChanged">
                            <asp:ListItem Text="Transfer Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                            <asp:ListItem Text="Split" Value="Split"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:TextBox ID="TypeDrpSelected" runat="server" CssClass="form-control" placeholder="Existing" ReadOnly="true" Visible="false"></asp:TextBox>
                        <button type="button" class="form-control" id="btnOpenModal" runat="server" data-toggle="modal" data-target="#exampleModalCenter" visible="false" onclick="handleSplitButtonClick()">
                            Split
                        </button>
                    </div>

                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="ToDistDrp" runat="server" AutoPostBack="true" class="form-control" Style="display: none;" onchange="showLoading()" OnSelectedIndexChanged="RouteTransToDistDrp_SelectedIndexChanged">
                        </asp:DropDownList>
                        <input type="text" id="ToDistSearch" runat="server" class="form-control" placeholder="Enter To Distributor" />
                        <%--<asp:DropDownList ID="ToDistDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="RouteTransToDistDrp_SelectedIndexChanged">
                            <asp:ListItem Text="To Distributor" Value=""></asp:ListItem>
                        </asp:DropDownList>--%>
                    </div>
                    <div id="btnDivSingle" class="col-12 col-md-3 mb-2 mb-md-0" runat="server" visible="true">
                        <asp:Button ID="RouteTransferSubmit" runat="server" Text="Transfer" CssClass="btn btn-success form-control"
                            OnClientClick="showLoading()" OnClick="RouteTransferSubmit_Click" />
                    </div>

                    <div id="btnDivSplit" class="col-12 col-md-3 mb-2 mb-md-0" runat="server" visible="false">
                        <div style="display: flex;">
                            <asp:Button ID="View" runat="server" Text="View" CssClass="btn btn-info form-control" Style="border-top-right-radius: 0; border-bottom-right-radius: 0; border-right: none; width: 30%; margin-right: 5px;"
                                OnClientClick="showLoading()" OnClick="View_Click" />
                            <asp:Button ID="RouteTransferSubmitH" runat="server" Text="Transfer" CssClass="btn btn-success form-control"
                                Style="border-top-left-radius: 0; border-bottom-left-radius: 0; width: 70%;"
                                OnClientClick="showLoading()" OnClick="RouteTransferSubmit_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="RouteTransExistGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;" OnRowCreated="RouteTransExistGridView_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                <asp:BoundField DataField="RouteName" HeaderText="RouteName" />
                                <asp:BoundField DataField="MnfCde" HeaderText="MNFCode" />
                                <asp:BoundField DataField="RouteTypeName" HeaderText="RouteType" />
                                <asp:BoundField DataField="CoverageName" HeaderText="RouteCoverage" />
                                <asp:BoundField DataField="WeekDay" HeaderText="Call Days" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input" style="position: relative; margin-left: -3px;" checked="checked" disabled="disabled" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="RouteTransSplitGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;" OnRowCreated="RouteTransSplitGridView_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                <asp:BoundField DataField="RouteName" HeaderText="RouteName" />
                                <asp:BoundField DataField="MnfCde" HeaderText="MNFCode" />
                                <asp:BoundField DataField="RouteTypeName" HeaderText="RouteType" />
                                <asp:BoundField DataField="CoverageName" HeaderText="RouteCoverage" />
                                <asp:BoundField DataField="WeekDay" HeaderText="Call Days" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />

                                <%--<asp:TemplateField>
                                    <HeaderTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="selectAllCheckBox" runat="server" style="margin-left: -3px;" class="form-check-input" onclick="selectAllCheckboxes(this)" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox" style="margin-left: -3px;"
                                                onclick="handleCheckboxClick(this)" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="RouteTransSplitRetailerGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;" OnRowCreated="RouteTransSplitRetailerGrid_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                <asp:BoundField DataField="RtrId" HeaderText="Retailer Id" />
                                <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                <asp:BoundField DataField="UrCode" HeaderText="UR Code" />

                                <asp:TemplateField>
                                    <%--<HeaderTemplate>
                            <div style="margin-right: 10px;">
                                <input type="checkbox" id="selectAllCheckBox" runat="server" style="margin-left: -3px;" class="form-check-input" onclick="selectAllCheckboxes(this)" />
                            </div>
                        </HeaderTemplate>--%>
                                    <ItemTemplate>
                                        <div style="margin-right: 10px;">
                                            <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox2" style="margin-left: -3px;" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="ToDistExistViewGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;" OnRowCreated="ToDistExistViewGrid_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                <asp:BoundField DataField="RouteName" HeaderText="RouteName" />
                                <asp:BoundField DataField="MnfCde" HeaderText="MNFCode" />
                                <asp:BoundField DataField="RouteTypeName" HeaderText="RouteType" />
                                <asp:BoundField DataField="CoverageName" HeaderText="RouteCoverage" />
                                <asp:BoundField DataField="WeekDay" HeaderText="Call Days" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <%-- Alert Modal for Retailer Existing Another DBR --%>
        <div class="modal fade" id="alertModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <%--<div class="modal-header">
                        <h5 class="modal-title" id="">Test Modal</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>--%>
                    <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                        <div class="form-group">
                            <%--<h5>Proceed to Transfer</h5>--%>
                            <asp:GridView ID="AlertModalGrid" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                <Columns>
                                    <asp:BoundField DataField="DISTCODE" HeaderText="Dist. Code" />
                                    <asp:BoundField DataField="DistNm" HeaderText="Dist. Name" />
                                    <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                    <asp:BoundField DataField="RetailerName" HeaderText="Retailer Name" />
                                    <asp:BoundField DataField="UrCode" HeaderText="URCode" />
                                    <asp:BoundField DataField="DistId" HeaderText="Dist ID" />

                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="margin-right: 10px;">
                                                <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox" style="margin-left: -3px;"
                                                    onclick="handleCheckboxClick(this)" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <HeaderStyle CssClass="header-hidden" />
                                <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                            </asp:GridView>
                            <br />
                            <br />
                            <h5 style="color: red">Retailers are already active in
                                <asp:Label ID="DBR" runat="server"></asp:Label>
                                DBR. Are you sure want to Proceed?</h5>
                            <br />
                            <h5 style="color: red">Note : </h5>
                            <p>
                                If you proceed, the mentioned retailers will be set to Inactive for DBR
                                <asp:Label ID="DBR2" runat="server"></asp:Label>.
                            </p>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="ProceedBtn" runat="server" Text="Proceed" CssClass="btn btn-success"
                            OnClientClick="showLoading()" OnClick="ProceedBtn_Click" />
                        <asp:Button ID="CancelBtn" runat="server" Text="Cancel" CssClass="btn btn-danger"
                            OnClientClick="showLoading()" OnClick="CancelBtn_Click" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Modal for Split Transfer --%>
        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">Routes</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                        <div class="form-group">
                            <asp:GridView ID="RouteSplitTransModal" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                <Columns>
                                    <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                    <asp:BoundField DataField="RouteName" HeaderText="RouteName" />

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="margin-right: 10px;">
                                                <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox" style="margin-left: -3px;"
                                                    onclick="handleCheckboxClick(this)" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header-hidden" />
                                <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-primary" onclick="selectItems()" data-dismiss="modal">Select</button>--%>
                        <asp:Button ID="SelectBtn" runat="server" Text="Select" CssClass="btn btn-primary"
                            OnClientClick="showLoading()" OnClick="SelectBtn_Click" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <%-- Notification Label --%>
        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />
    </form>

    <%-- Script for selectall checkboxes in RouteTransferSplitGridview --%>
    <%--<script type="text/javascript">
        function selectAllCheckboxes(source) {
            var checkboxes = document.querySelectorAll('.rowCheckbox');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>--%>

    <%--script for SplitGridView Checkboxes to remain any one checkbox--%>
    <%--<script type="text/javascript">
        // Function to handle checkbox click
        function handleCheckboxClick(checkbox) {
            // Get all checkboxes
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            // Count checked checkboxes
            const checkedCount = Array.from(checkboxes).filter(cb => cb.checked).length;

            // Apply the logic to ensure at least one checkbox remains unchecked
            checkboxes.forEach(cb => {
                if (!cb.checked && checkedCount === checkboxes.length - 1) {
                    cb.disabled = true; // Disable the last unchecked checkbox
                } else {
                    cb.disabled = false; // Enable others
                }
            });
        }

        // This function is triggered when the page is loaded to ensure proper checkbox states
        window.onload = function () {
            const checkboxes = document.querySelectorAll('.rowCheckbox');
            const checkedCount = Array.from(checkboxes).filter(cb => cb.checked).length;

            // Apply the logic to ensure at least one checkbox remains unchecked
            checkboxes.forEach(cb => {
                if (!cb.checked && checkedCount === checkboxes.length - 1) {
                    cb.disabled = true; // Disable the last unchecked checkbox
                } else {
                    cb.disabled = false; // Enable others
                }
            });
        }
    </script>--%>

    <script type="text/javascript">
        // This function is triggered when the Split button is clicked
        function handleSplitButtonClick() {
            setCheckboxState(); // Check if checkboxes need to be disabled when Modal opens
        }

        // Function to handle checkbox click
        function handleCheckboxClick(checkbox) {
            // Get all checkboxes
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            // Count checked checkboxes
            const checkedCount = Array.from(checkboxes).filter(cb => cb.checked).length;

            // Apply the logic to ensure at least one checkbox remains unchecked
            checkboxes.forEach(cb => {
                if (!cb.checked && checkedCount === checkboxes.length - 1) {
                    cb.disabled = true; // Disable the last unchecked checkbox

                    // Show toast notification
                    showToast("At least one Route will remain in Split case", "toast-warning");

                } else {
                    cb.disabled = false; // Enable others
                }
            });
        }

        // This function is triggered when the page or modal is loaded to ensure proper checkbox states
        function setCheckboxState() {
            const checkboxes = document.querySelectorAll('.rowCheckbox');

            if (checkboxes.length === 1) {
                // Disable the checkbox if there's only one checkbox in the modal
                checkboxes[0].disabled = true;
            } else {
                // Enable all checkboxes if there are multiple checkboxes
                checkboxes.forEach(cb => cb.disabled = false);
            }
        }

        // Ensure proper checkbox state on modal open
        window.onload = function () {
            setCheckboxState();
        }
</script>

    <script>
        function showModal() {
            $('#alertModal').modal('show');
        }
    </script>

    <%--scri[pt for progressbar--%>
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
            $('#<%= ToDistDrp.ClientID %> option').each(function () {
                var text = $(this).text();  // DistNm
                var value = $(this).val();  // DistCode
                if (value) {
                    options.push({ label: text, value: value });
                }
            });

            // Initialize jQuery UI Autocomplete
            $('#ToDistSearch').autocomplete({
                source: options,
                select: function (event, ui) {
                    // Set the selected value in the input box (DistNm)
                    $('#ToDistSearch').val(ui.item.label);

                    // Update the hidden DropDownList (ToDistDrp) value
                    $('#<%= ToDistDrp.ClientID %>').val(ui.item.value);

                    // Trigger OnSelectedIndexChanged event of DropDownList
                    __doPostBack('<%= ToDistDrp.UniqueID %>', '');

                    showLoading();

                    // Return true to avoid clearing the selected value
                    return true; // Ensures value remains in the input field
                }
            });
        });

    </script>
</body>
</html>
