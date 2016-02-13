angular.module("umbraco").controller("socialFacebookScopesDialogCtrl", function ($scope, $http) {

    var selection = Array.isArray($scope.dialogOptions.selection) ? $scope.dialogOptions.selection : [];

    $http.get('/umbraco/SkybrudSocial/Facebook/GetScopes').success(function (r) {
        $scope.scopes = r;
        angular.forEach($scope.scopes, function (group) {
            angular.forEach(group.scopes, function (scope) {
                scope.checked = selection.indexOf(scope.Name) >= 0;
            });
        });
    });

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

});