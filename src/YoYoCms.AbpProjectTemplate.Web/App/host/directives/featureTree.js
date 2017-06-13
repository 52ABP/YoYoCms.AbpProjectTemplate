/* Used by edition and tenant features. 
 */
appModule.directive('featureTree', [
    function () {
        return {
            restrict: 'E',
            template: '<div class="feature-tree"></div>',
            scope: {
                editData: '='
            },
            link: function ($scope, element, attr) {

                var $tree = $(element).find('.feature-tree');
                var treeInitializedBefore = false;
                var inTreeChangeEvent = false;

                $scope.$watch('editData', function () {
                    if (!$scope.editData) {
                        return;
                    }

                    $scope.editData.isValid = function() {
                        return areAllValuesValid();
                    };

                    initializeTree();
                });

                function initializeTree() {
                    if (treeInitializedBefore) {
                        $tree.jstree('destroy');
                    }

                    treeInitializedBefore = true;

                    var treeData = _.map($scope.editData.features, function (feature) {
                        return {
                            id: feature.name,
                            parent: feature.parentName ? feature.parentName : '#',
                            text: feature.displayName,
                            state: {
                                opened: true,
                                selected: isFeatureEnabled(feature.name)
                            }
                        };
                    });

                    $tree
                        .on('ready.jstree', function () {
                            customizeTreeNodes();
                        })
                        .on('redraw.jstree', function () {
                            customizeTreeNodes();
                        })
                        .on('after_open.jstree', function () {
                            customizeTreeNodes();
                        })
                        .on('create_node.jstree', function () {
                            customizeTreeNodes();
                        })
                        .on("changed.jstree", function (e, data) {
                            if (!data.node) {
                                return;
                            }

                            var wasInTreeChangeEvent = inTreeChangeEvent;
                            if (!wasInTreeChangeEvent) {
                                inTreeChangeEvent = true;
                            }

                            var childrenNodes;

                            if (data.node.state.selected) {
                                selectNodeAndAllParents($tree.jstree('get_parent', data.node));

                                childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                                $tree.jstree('select_node', childrenNodes);

                            } else {
                                childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                                $tree.jstree('deselect_node', childrenNodes);
                            }

                            if (!wasInTreeChangeEvent) {
                                var $nodeLi = getNodeLiByFeatureName(data.node.id);
                                var feature = findFeatureByName(data.node.id);
                                if (feature && (!feature.inputType || feature.inputType.name == 'CHECKBOX')) {
                                    var value = $tree.jstree('is_checked', $nodeLi) ? 'true' : 'false';
                                    setFeatureValueByName(data.node.id, value);
                                }

                                inTreeChangeEvent = false;
                            }
                        }).jstree({
                            'core': {
                                data: treeData
                            },
                            "types": {
                                "default": {
                                    "icon": "fa fa-folder tree-item-icon-color icon-lg"
                                },
                                "file": {
                                    "icon": "fa fa-file tree-item-icon-color icon-lg"
                                }
                            },
                            'checkbox': {
                                keep_selected_style: false,
                                three_state: false,
                                cascade: ''
                            },
                            plugins: ['checkbox', 'types']
                        });
                };

                function customizeTreeNodes() {
                    $tree.find('.jstree-node').each(function () {
                        var $nodeLi = $(this);
                        var $nodeA = $nodeLi.find('.jstree-anchor');

                        var featureName = $nodeLi.attr('id');
                        var feature = findFeatureByName(featureName);
                        var featureValue = findFeatureValueByName(featureName) || '';

                        if (!feature || !feature.inputType) {
                            return;
                        }

                        if (feature.inputType.name == 'CHECKBOX') {
                            //no change for checkbox
                        } else if (feature.inputType.name == 'SINGLE_LINE_STRING') {
                            if (!$nodeLi.find('.feature-tree-textbox').length) {
                                $nodeA.find('.jstree-checkbox').hide();

                                var inputType = 'text';
                                if (feature.inputType.validator) {
                                    if (feature.inputType.validator.name == 'NUMERIC') {
                                        inputType = 'number';
                                    }
                                }

                                var $textbox = $('<input class="feature-tree-textbox" type="' + inputType + '" />')
                                    .val(featureValue);

                                if (inputType == 'number') {
                                    $textbox.attr('min', feature.inputType.validator.minValue);
                                    $textbox.attr('max', feature.inputType.validator.maxValue);
                                } else {
                                    if (feature.inputType.validator && feature.inputType.validator.name == 'STRING') {
                                        if (feature.inputType.validator.maxLength > 0) {
                                            $textbox.attr('maxlength', feature.inputType.validator.maxLength);
                                        }
                                        if (feature.inputType.validator.minLength > 0) {
                                            $textbox.attr('required', 'required');
                                        }
                                        if (feature.inputType.validator.regularExpression) {
                                            $textbox.attr('pattern', feature.inputType.validator.regularExpression);
                                        }
                                    }
                                }

                                $textbox.on('input propertychange paste', function () {
                                    if (isFeatureValueValid(featureName, $textbox.val())) {
                                        setFeatureValueByName(featureName, $textbox.val());
                                        $textbox.removeClass('feature-tree-textbox-invalid');
                                    } else {
                                        $textbox.addClass('feature-tree-textbox-invalid');
                                    }
                                });

                                $textbox.appendTo($nodeLi);
                            }
                        } else if (feature.inputType.name == 'COMBOBOX') {
                            if (!$nodeLi.find('.feature-tree-combobox').length) {
                                $nodeA.find('.jstree-checkbox').hide();

                                var $combobox = $('<select class="feature-tree-combobox" />');
                                _.each(feature.inputType.itemSource.items, function (opt) {
                                    $('<option></option>')
                                        .attr('value', opt.value)
                                        .text(opt.displayText)
                                        .appendTo($combobox);
                                });

                                $combobox
                                    .val(featureValue)
                                    .on('change', function () {
                                        setFeatureValueByName(featureName, $combobox.val());
                                    })
                                    .appendTo($nodeLi);
                            }
                        }
                    });
                }

                function getNodeLiByFeatureName(featureName) {
                    return $('#' + featureName.replace('.', '\\.'));
                }

                function selectNodeAndAllParents(node) {
                    $tree.jstree('select_node', node, true);
                    var parent = $tree.jstree('get_parent', node);
                    if (parent) {
                        selectNodeAndAllParents(parent);
                    }
                };

                function findFeatureByName(featureName) {
                    var feature = _.find($scope.editData.features, function (f) { return f.name == featureName });

                    if (!feature) {
                        abp.log.warn('Could not find a feature by name: ' + featureName);
                    }

                    return feature;
                }

                function findFeatureValueByName(featureName) {
                    var feature = findFeatureByName(featureName);
                    if (!feature) {
                        return '';
                    }

                    var featureValue = _.find($scope.editData.featureValues, function (f) { return f.name == featureName });
                    if (!featureValue) {
                        return feature.defaultValue;
                    }

                    return featureValue.value;
                }

                function isFeatureValueValid(featureName, value) {
                    var feature = findFeatureByName(featureName);
                    if (!feature || !feature.inputType || !feature.inputType.validator) {
                        return true;
                    }

                    var validator = feature.inputType.validator;
                    if (validator.name == 'STRING') {
                        if (value == undefined || value == null) {
                            return validator.allowNull;
                        }

                        if (typeof value != 'string') {
                            return false;
                        }

                        if (validator.minLength > 0 && value.length < validator.minLength) {
                            return false;
                        }

                        if (validator.maxLength > 0 && value.length > validator.maxLength) {
                            return false;
                        }

                        if (validator.regularExpression) {
                            return (new RegExp(validator.regularExpression)).test(value);
                        }
                    } else if (validator.name == 'NUMERIC') {
                        var numValue = parseInt(value);

                        if (isNaN(numValue)) {
                            return false;
                        }

                        var minValue = validator.minValue;
                        if (minValue > numValue) {
                            return false;
                        }

                        var maxValue = validator.maxValue;
                        if (maxValue > 0 && numValue > maxValue) {
                            return false;
                        }
                    }

                    return true;
                }

                function areAllValuesValid() {
                    //Refresh checkbox values
                    $tree.find('.jstree-node').each(function () {
                        var $nodeLi = $(this);
                        var featureName = $nodeLi.attr('id');
                        var feature = findFeatureByName(featureName);

                        if (feature && (!feature.inputType || feature.inputType.name == 'CHECKBOX')) {
                            var value = $tree.jstree('is_checked', $nodeLi) ? 'true' : 'false';
                            setFeatureValueByName(featureName, value);
                        }
                    });

                    return $tree.find('.feature-tree-textbox-invalid').length <= 0;
                }

                function setFeatureValueByName(featureName, value) {
                    var featureValue = _.find($scope.editData.featureValues, function (f) { return f.name == featureName });
                    if (!featureValue) {
                        return;
                    }

                    $scope.$root.safeApply(function () {
                        featureValue.value = value;
                    });
                }

                function isFeatureEnabled(featureName) {
                    var value = findFeatureValueByName(featureName);
                    return value.toLowerCase() == 'true';
                }
            }
        };
    }
]);