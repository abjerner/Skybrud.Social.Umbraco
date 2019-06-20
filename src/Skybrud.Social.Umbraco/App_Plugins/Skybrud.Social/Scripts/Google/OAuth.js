angular.module("umbraco").controller("Skybrud.Social.Google.OAuth.Controller", function ($scope, $element, editorState) {

    // Get the server variables for "Skybrud.Social"
    var variables = window.Umbraco.Sys.ServerVariables.skybrudSocial;

    $scope.editorState = editorState.current;

    // Define an alias for the editor (eg. used for callbacks)
    var alias = $scope.alias = ("skybrudsocial_" + Math.random()).replace(".", "");

    $scope.authenticate = function () {
        var url = variables.surfaceBaseUrl + "GoogleOAuth/Authenticate?callback=" + alias + "&contentTypeAlias=" + editorState.current.contentTypeAlias + "&propertyAlias=" + $scope.model.alias;
        window.open(url, "Authenticate with Google", "scrollbars=no,resizable=yes,menubar=no,width=800,height=700");
    };

    $scope.reset = function () {
        $scope.model.value = null;
    };

    window[alias] = function (data) {
        $scope.$apply(function () {
            $scope.model.value = data;
        });
    }

});