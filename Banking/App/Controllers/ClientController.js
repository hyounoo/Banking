'use strict'
app.controller('ClientController', ['$scope', '$http', '$filter', function ($scope, $http, $filter) {
    $scope.model = {
        currencies: [],
        selectedCurrency: null,
        clientId: null,
        clientName: null,
        connectionId: null,
        accounts: [],
        selectedAccount: null,
        selectedAccountCurrency: null,
        amount: null,
        canDepositOrWithdraw: false,
        transferAccountNumber: null,
        transferAccountTitle: null,
        transferClientId: null,
        transferAmount: null
    };

    $scope.fn = {};

    $scope.fn.getCurrencyInfo = function () {
        $http({
            url: rootPath + 'api/currencies',
            method: "GET"
        }).then(function (response) {
            if (response.status == 200) {
                $scope.model.currencies = response.data;
            } else {
                alert(response.statusText);
            }
        });
    }

    $scope.fn.getClientInfo = function () {
        $http({
            url: rootPath + 'api/clients',
            method: "GET",
            params: {
                id: $scope.model.clientId
            }
        }).then(function (response) {
            if (response.status == 200) {
                $scope.model.clientName = response.data.Name;
                $scope.model.accounts = response.data.Accounts;

                if ($scope.model.selectedAccount) {
                    var selectedAccountNumber = $scope.model.selectedAccount.AccountNumber;

                    angular.forEach($scope.model.accounts, function (value, key) {
                        if (value.AccountNumber == selectedAccountNumber) {
                            $scope.model.selectedAccount = value;
                        }
                    });
                }
            } else {
                alert(response.statusText);
            }
        });
    }

    $scope.fn.balance = function () {
        $http({
            url: rootPath + 'api/accounts/balance',
            method: "GET",
            params: {
                accountNumber: $scope.model.selectedAccount.AccountNumber
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {
                $scope.model.selectedAccount.Balance = data.Balance;

                $('#messages').append('<li><strong> Balance of account "' + $scope.model.selectedAccount.Title + '": ' + $scope.model.selectedAccount.Balance + '</strong></li>');

            } else {
                alert(data.Message);
            }
        });
    }

    $scope.fn.deposit = function (accountNumber, title, amount, clientId) {
        var isTransfer = $scope.model.selectedAccount.AccountNumber != accountNumber;
        $http({
            url: rootPath + 'api/accounts/deposit',
            method: "POST",
            params: {
                accountNumber: accountNumber,
                amount: amount,
                currency: isTransfer ? $scope.model.selectedCurrency.CurrencyCode : $scope.model.selectedAccountCurrency.CurrencyCode
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {
                if ($scope.model.selectedAccount.AccountNumber == accountNumber) {
                    $scope.model.selectedAccount.Balance = data.Balance;
                } else {
                    bank.server.updateClient($scope.model.selectedAccount.AccountNumber, accountNumber, amount, $scope.model.selectedCurrency.CurrencyCode);
                }

                $('#messages').append('<li><strong> Deposit to "' + title + '" for ' + $scope.model.selectedCurrency.CurrencyCode + ' ' + amount + '.</strong></li>');
            } else {
                alert(data.Message);
            }
        });
    }

    $scope.fn.withdraw = function (accountNumber, title, amount, isTransfer) {
        $http({
            url: rootPath + 'api/accounts/withdraw',
            method: "POST",
            params: {
                accountNumber: accountNumber,
                amount: amount,
                currency: isTransfer ? $scope.model.selectedCurrency.CurrencyCode : $scope.model.selectedAccountCurrency.CurrencyCode
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {
                $scope.model.selectedAccount.Balance = data.Balance;

                var currency = isTransfer ? $scope.model.selectedCurrency.CurrencyCode : $scope.model.selectedAccountCurrency.CurrencyCode;
                $('#messages').append('<li><strong> Withdraw from "' + title + '" for ' + currency + ' ' + amount + '.</strong></li>');

                if (isTransfer) {
                    $scope.fn.deposit($scope.model.transferAccountNumber, $scope.model.transferAccountTitle, $scope.model.transferAmount, $scope.model.transferClientId);
                }
            } else {
                alert(data.Message);
            }
        });
    }

    $scope.fn.transfer = function () {
        $http({
            url: rootPath + 'api/accounts',
            method: "GET",
            params: {
                accountNumber: $scope.model.transferAccountNumber
            }
        }).then(function (response) {
            if (response.status == 200) {
                $scope.model.transferAccountTitle = response.data.Title;
                $scope.model.transferClientId = response.data.ClientId;

                $('#messages').append('<br />');
                $('#messages').append('<li><strong> Preparing transfer to "' + response.data.Title + '" for ' + $scope.model.transferAmount + '</strong></li>');
                $scope.fn.withdraw($scope.model.selectedAccount.AccountNumber, $scope.model.selectedAccount.Title, $scope.model.transferAmount, true);                
            }
            else {
                $('#messages').append('<li><strong> Error code: ' + response.status + '.</strong></li>');
            }
        }, function (response) {
            $('#messages').append('<li><strong> Error code: ' + response.status + '.</strong></li>');
            $('#messages').append('<li><strong> Error message: ' + response.statusText + '.</strong></li>');
        });
    }

    $scope.fn.selectedAccountChanged = function () {
        $scope.model.amount = null;

        $scope.model.selectedAccountCurrency = $filter('filter')($scope.model.currencies, { 'Id': $scope.model.selectedAccount.CurrencyId })[0];
        $scope.model.selectedCurrency = $filter('filter')($scope.model.currencies, { 'Id': $scope.model.selectedAccount.CurrencyId })[0];
    }

    angular.element(document).ready(function () {

        $scope.model.clientId = angular.fromJson($('#ClientId').val());

        $scope.fn.getCurrencyInfo();

        // Start the connection.
        $.connection.hub.start();

        // Signal Client functions

        // Client is connected.
        bank.client.Connected = function () {
            bank.server.logIn($scope.model.clientId);
        }

        // Declares message function when login succeed.
        bank.client.logedIn = function (connectionId) {
            $scope.model.connectionId = connectionId;
            $('#messages').append('<li><strong> Your connection ID:' + $scope.model.connectionId + '</strong></li>');

            // Get Client info.
            $scope.fn.getClientInfo();
        };

        bank.client.logedOut = function () {
            $('#messages').append('<li><strong> You are loged out from server.</strong></li>');
        };

        bank.client.updateClient = function (sender, reciever, amount, currencyCode) {
            $('#messages').append('<li><strong> ' + sender + ' has transfered amount of ' + currencyCode + ' ' + amount + ' to ' + reciever + '.</strong></li>');
            $scope.fn.getClientInfo();
        }
    });
}]);