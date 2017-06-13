var FeaturesTree = (function ($) {
    return function() {
        var $tree;

        function init($treeContainer) {
            $tree = $treeContainer;

            $tree.on('ready.jstree', function() {
                    customizeTreeNodes();
                })
                .on('redraw.jstree', function() {
                    customizeTreeNodes();
                })
                .on('after_open.jstree', function() {
                    customizeTreeNodes();
                })
                .on('create_node.jstree', function() {
                    customizeTreeNodes();
                }).on("changed.jstree", function(e, data) {
                    if (!data.node) {
                        return;
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
                }).jstree({
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

            function customizeTreeNodes() {
                $tree.find('.jstree-node').each(function () {
                    var $nodeLi = $(this);
                    var $nodeA = $nodeLi.find('.jstree-anchor');
                    var feature = JSON.parse($nodeLi.attr('data-feature'));
                    var featureValue = $nodeLi.attr('data-feature-value');

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
                                if (isFeatureValueValid(feature, $textbox.val())) {
                                    $nodeLi.attr('data-feature-value', $textbox.val());
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
                                    $nodeLi.attr('data-feature-value', $combobox.val());
                                })
                                .appendTo($nodeLi);
                        }
                    }
                });
            }
        };

        function selectNodeAndAllParents(node) {
            $tree.jstree('select_node', node, true);
            var parent = $tree.jstree('get_parent', node);
            if (parent) {
                selectNodeAndAllParents(parent);
            }
        };
        
        function isFeatureValueValid(feature, value) {
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

        function isValid() {
            return $tree.find('.feature-tree-textbox-invalid').length <= 0;
        }

        function getFeatureValues() {
            var featureValues = [];

            $tree.find('.jstree-node').each(function() {
                var $nodeLi = $(this);
                var feature = JSON.parse($nodeLi.attr('data-feature'));
                if (!feature.inputType || feature.inputType.name == 'CHECKBOX') {
                    featureValues.push({
                        name: feature.name,
                        value: $tree.jstree('is_checked', $nodeLi) ? 'true' : 'false'
                    });
                } else {
                    featureValues.push({
                        name: feature.name,
                        value: $nodeLi.attr('data-feature-value')
                    });
                }
            });

            return featureValues;
        };

        return {
            init: init,
            getFeatureValues: getFeatureValues,
            isValid: isValid
        }
    }
})(jQuery);