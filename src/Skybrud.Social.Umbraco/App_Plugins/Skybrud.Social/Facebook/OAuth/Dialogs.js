angular.module("umbraco").controller("socialFacebookScopesDialogCtrl", function ($scope, $http) {

    var selection = Array.isArray($scope.dialogOptions.selection) ? $scope.dialogOptions.selection : [];

    $scope.count = 0;

    $scope.update = function() {
        var count = 0;
        angular.forEach($scope.scopes, function(group) {
            angular.forEach(group.scopes, function(scope) {
                if (scope.checked) count++;
            });
        });
        $scope.count = count;
    };

    $scope.confirm = function() {
        var temp = [];
        angular.forEach($scope.scopes, function(group) {
            angular.forEach(group.scopes, function(scope) {
                if (scope.checked) {
                    temp.push(scope.Name);
                }
            });
        });
        $scope.submit(temp);
    };

    $http.get('/umbraco/SkybrudSocial/Facebook/GetScopes').success(function (r) {
        $scope.scopes = r;
        var count = 0;
        angular.forEach($scope.scopes, function (group) {
            angular.forEach(group.scopes, function (scope) {
                scope.checked = selection.indexOf(scope.Name) >= 0;
                if (scope.checked) {
                    count++;
                }
            });
        });
        $scope.count = count;
    });

});