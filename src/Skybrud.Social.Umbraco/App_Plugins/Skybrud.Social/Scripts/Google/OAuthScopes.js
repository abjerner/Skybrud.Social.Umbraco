angular.module("umbraco").controller("Skybrud.Social.Google.OAuthScopes.Controller", function ($scope, $http, editorService) {

    // Get the server variables for "Skybrud.Social"
    var variables = window.Umbraco.Sys.ServerVariables.skybrudSocial;

    // Make sure we have an array for the selected scopes
    if (!Array.isArray($scope.model.value)) $scope.model.value = ["email", "profile"];

    // Shadow array for keeping track of the selected scopes
    $scope.scopes = [];

    $scope.loading = true;

    $http.get(variables.webApiBaseUrl + "Google/GetScopes").then(function(res) {

        $scope.scopes = res.data;

        $scope.scopes.forEach(function (group, g) {
            group.count = 0;
            group.expanded = g === 0;
            group.scopes.forEach(function (scope) {
                scope.selected = $scope.model.value.indexOf(scope.alias) >= 0;
                if (scope.selected) {
                    group.expanded = true;
                    group.count++;
                }
            });
        });

        $scope.loading = false;

    });

    $scope.toggle = function (scope) {

        scope.selected = !scope.selected;

        update();

    };

    function update() {

        var temp = [];

        $scope.scopes.forEach(function (group) {
            group.count = 0;
            group.scopes.forEach(function (scope) {
                if (scope.selected) {
                    temp.push(scope.alias);
                    group.count++;
                }
            });
        });

        $scope.model.value = temp;

    }

});