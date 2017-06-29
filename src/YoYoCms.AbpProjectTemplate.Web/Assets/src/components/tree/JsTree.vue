<!--对jstree的封装-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <article class="jstree--container" ref="container">
        加载中...
    </article>
</template>

<script>
    export default {
        props: {
            treeData: Array,
            plugins: Array,
            context: Object, // 组件的上下文
            onItemClick: Function, // 每一项点击后回调
        },
        data() {
            return {}
        },
        created() {
        },
        activated() {
            this.$emit('update:context', this)
        },
        methods: {
            // 初始化
            init () {
                this.$nextTick(() => {
                    $(this.$refs.container).jstree('destroy')
                    $(this.$refs.container).on('changed.jstree', (e, data) => {
                        let i
                        let j
                        let ret = []
                        for (i = 0, j = data.selected.length; i < j; i++) {
                            ret.push(data.instance.get_node(data.selected[i]))
                        }
                        this.onItemClick && this.onItemClick(ret[0])
                    }).jstree({
                        'core': {
                            data: this.treeData
                        },
                        'types': {
                            '#': {
                                'max_children': 1,
                                'max_depth': 4,
                                'valid_children': ['root']
                            },
                            'root': {
                                'icon': '/static/3.3.4/assets/images/tree_icon.png',
                                'valid_children': ['default']
                            },
                            'default': {
                                'valid_children': ['default', 'file']
                            },
                            'file': {
                                'icon': 'glyphicon glyphicon-file',
                                'valid_children': []
                            }
                        },
                        'checkbox': {
                            keep_selected_style: false,
                            three_state: false,
                            cascade: ''
                        },
                        plugins: this.plugins || []
                    })
                })
            },
            // 获取选中的数组
            get_selected () {
                return $(this.$refs.container).jstree('get_selected')
            }
        },
        components: {}
    }
</script>
