angular.module('bankingApp', [])
    .controller('ClientController', ['$scope', '$http', function ($scope, $http) {

        $scope.accounts = [];        

        $scope.checkBalance = function () {
            alert("balance");
        };

        var bank = $.connection.bankingHub;
        // Create a function that the hub can call back to display messages.
        bank.client.updateAccounts = function (status) {
            // Add the message to the page.
            $('#messages').append('<li><strong> Status:' + htmlEncode(status) + '</strong></li>');
        };

        // Declares message function when login succeed.
        bank.client.logedIn = function (connectionId) {
            $('#messages').append('<li><strong> Your connection ID:' + htmlEncode(connectionId)
                + '</strong></li>');
        };

        // Declares message function when login failed.
        bank.client.failedLogIn = function (userId) {
            $('#messages').append('<li><strong> Client Id:' + htmlEncode(userId)
                + '</strong> is already loged in.</li>');
        };

        // Start the connection.
        $.connection.hub.start().done(function () {
            // On connection try login with userId.
            bank.server.logIn($('#clientId').val());
        });

        function htmlEncode(value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        }
        $scope.var = {};
        $scope.fn = {};

        $scope.var.todos = [
            { text: 'learn AngularJS', done: true },
            { text: 'build an AngularJS app', done: false }
        ];

        $scope.fn.addTodo = function () {
            todoList.todos.push({ text: todoList.todoText, done: false });
            todoList.todoText = '';
        };

        $scope.fn.remaining = function () {
            var count = 0;
            angular.forEach(todoList.todos, function (todo) {
                count += todo.done ? 0 : 1;
            });
            return count;
        };

        $scope.fn.archive = function () {
            var oldTodos = todos;
            todoList.todos = [];
            angular.forEach(oldTodos, function (todo) {
                if (!todo.done) todoList.todos.push(todo);
            });
        };
    }]);
