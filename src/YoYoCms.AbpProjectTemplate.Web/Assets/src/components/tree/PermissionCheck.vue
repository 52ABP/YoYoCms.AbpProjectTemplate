<!--权限树 可选择-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <JsTree ref="jstree" :treeData="treeData" :plugins="plugins"></JsTree>
</template>

<script>
    import JsTree from './JsTree.vue'
    export default {
        props: {
            orignPermission: Object,
            context: Object
        },
        data() {
            return {
                permissions: void 0, // 所有的权限
                grantedPermissionNames: void 0, // 当前用户拥有的权限
                treeData: [],
                plugins: ['checkbox', 'types'], // jstree的plugins参数
            }
        },
        watch: {
            'orignPermission'(val) {
                this.treeData = []
                // 获取所有的权限信息
                let retPermission = this.orignPermission // await userService.getUserPermissionsForEdit({id: this.userid})
                this.permissions = retPermission.permissions
                this.grantedPermissionNames = retPermission.grantedPermissionNames
                this.permissions.forEach((item) => {
                    this.treeData.push({
                        id: item.name,
                        parent: item.parentName ? item.parentName : '#',
                        text: item.displayName,
                        state: {
                            opened: true,
                            selected: this.grantedPermissionNames.indexOf(item.name) > -1 // _.contains($scope.editData.grantedPermissionNames, item.name)
                        }
                    })
                })

                this.$nextTick(() => {
                    this.$refs.jstree.init()
                })
            }
        },
        created () {
            this.$emit('update:context', this)
        },
        async activated() {
        },
        async mounted () {
        },
        methods: {
            get_selected() {
                return this.$refs.jstree.get_selected()
            }
        },
        components: {JsTree}
    }
</script>
