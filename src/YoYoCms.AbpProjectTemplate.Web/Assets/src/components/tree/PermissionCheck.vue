<!--权限树 可选择-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <article class="permission-check--container" v-loading="loading">
        <el-dialog ref="dialog"
                   @open="handleOpen"
                   :title="title"
                   :visible.sync="dialogVisible"
                   size="tiny">

            <JsTree ref="jstree" :treeData="treeData" :plugins="plugins"></JsTree>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="confirmClick">确 定</el-button>
          </span>
        </el-dialog>
    </article>
</template>

<script>
    import userService from '../../services/userService'

    import JsTree from './JsTree.vue'
    export default {
        props: {
            title: String,
            value: Boolean,
            userid: Number,
            onConfirmCb: Function, // 确定按钮的回调 参数为 选中的权限id集合
        },
        data() {
            return {
                dialogVisible: this.value,
                loading: false,
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
//                user: this.$store.state.auth.user,
                permissions: void 0, // 所有的权限
                grantedPermissionNames: void 0, // 当前用户拥有的权限
                treeData: [],
                plugins: ['checkbox', 'types'], // jstree的plugins参数
            }
        },
        watch: {
            'value' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('input', val)
            }
        },
        async activated() {
        },
        async mounted () {
        },
        methods: {
            fetchData () {
            },
            async handleOpen () {
                this.loading = true
                this.treeData = []
                // 获取所有的权限信息
                let retPermission = await userService.getUserPermissionsForEdit({id: this.userid})
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
                    this.$refs.dialog.$children[2].init()
                })
                this.loading = false
            },
            // 确定按钮点击回调
            confirmClick () {
                if (!this.onConfirmCb) this.dialogVisible = false

                this.loading = true
                this.onConfirmCb(this.$refs.dialog.$children[2].get_selected()).then(() => {
                    this.dialogVisible = false
                    this.loading = false
                })
            }
        },
        components: {JsTree}
    }
</script>
