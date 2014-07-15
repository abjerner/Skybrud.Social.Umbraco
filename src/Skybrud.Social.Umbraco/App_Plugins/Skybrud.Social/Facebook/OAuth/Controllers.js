angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.expiresDays = null;

    $scope.callback = function (data) {

        $scope.model.value = data;

        updateUI();

    };

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Facebook OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };
    
    function format(number, decimals) {
        var text = Math.round(number * 10 * decimals);
        return text / 10 / decimals;
    }
    
    function updateUI() {

        if ($scope.model.value && $scope.model.value.expires_at) {

            var seconds = (new Date($scope.model.value.expires_at) - new Date()) / 1000;
            $scope.expiresDays = format(seconds / 60 / 60 / 24, 2);

        } else {

            $scope.expiresDays = NaN;

        }

    }

    updateUI();

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.PreValues.Controller", ['$scope', 'assetsService', function ($scope, assetsService) {
    
    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: ''
        };
    }

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx';

    assetsService.loadCss("/App_Plugins/Skybrud.Social/Social.css");

}]);