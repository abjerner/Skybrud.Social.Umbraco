angular.module("umbraco").controller("Skybrud.Social.Google.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.callback = function (data) {
        $scope.$apply(function () {
            $scope.model.value = data;
        });
    };

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Google OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Google.OAuth.PreValues.Controller", ['$scope', '$http', 'dialogService', function ($scope, $http, dialogService) {

    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: ''
        };
    }

    // Make sure we have an array for the selected scopes
    if (!Array.isArray($scope.model.value.scope)) $scope.model.value.scope = ['email', 'profile'];

    // Shadow array for keeping track of the selected scopes
    $scope.scopes = [];

    // Updates the model based on the shadow array
    function updateModel() {
        var temp = [];
        angular.forEach($scope.scopes, function (scope) {
            temp.push(scope.alias);
        });
        $scope.model.value.scope = temp;
    }

    $scope.addScope = function () {

        var d = dialogService.open({
            modalClass: 'SocialDialog',
            template: '/App_Plugins/Skybrud.Social/Google/OAuth/ScopesDialog.html',
            show: true,
            callback: function (scopes) {
                $scope.scopes = scopes;
                updateModel();
            },
            selection: $scope.model.value.scope
        });

        d.element[0].style.width = '1000px';
        d.element[0].style.marginLeft = '-500px';

    };

    $scope.removeScope = function (index) {
        $scope.scopes.splice(index, 1);
        updateModel();
    };

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx';

    // Fetch all scopes from the API so we can look up the selected scopes
    $http.get('/umbraco/SkybrudSocial/Google/GetScopes').success(function (r) {
        angular.forEach(r, function (group) {
            angular.forEach(group.scopes, function (scope) {
                if ($scope.model.value.scope.indexOf(scope.alias) !== -1) {
                    $scope.scopes.push(scope);
                }
            });
        });
    });

}]);