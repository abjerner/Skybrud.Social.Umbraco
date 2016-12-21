angular.module("umbraco").controller("socialGoogleScopesDialogCtrl", function ($scope, $http) {

    var selection = Array.isArray($scope.dialogOptions.selection) ? $scope.dialogOptions.selection : [];

    $scope.count = 0;

    $scope.update = function () {
        var count = 0;
        angular.forEach($scope.scopes, function (group) {
            angular.forEach(group.scopes, function (scope) {
                if (scope.checked) count++;
            });
        });
        $scope.count = count;
    };

    $scope.confirm = function () {
        var temp = [];
        angular.forEach($scope.scopes, function (group) {
            angular.forEach(group.scopes, function (scope) {
                if (scope.checked) {
                    temp.push(scope);
                }
            });
        });
        $scope.submit(temp);
    };

    $http.get('/umbraco/SkybrudSocial/Google/GetScopes').success(function (r) {
        $scope.scopes = r;
        var count = 0;
        angular.forEach($scope.scopes, function (group) {
            angular.forEach(group.scopes, function (scope) {

                // Define a unique ID for each scope (used for the label in the UI)
                scope.id = ('skybrudsocial_scope_' + Math.random()).replace('.', '');

                // Is the scope selected?
                scope.checked = selection.indexOf(scope.alias) >= 0;

                // Increment the counter if selected
                if (scope.checked) count++;

            });
        });
        $scope.count = count;
    });

});