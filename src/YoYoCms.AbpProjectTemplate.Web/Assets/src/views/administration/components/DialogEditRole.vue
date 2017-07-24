<!--修改角色的弹出框-->
<style rel="styleesheet" lang="scss">
    .edit-role--dialog {
        .el-dialog {
            top: 15px !important;
        }
    }
</style>

<template>
    <el-dialog class="edit-role--dialog"
               :title="currRole.id? L('EditRole') + ':' + currRole.displayName : L('CreateNewRole')"
               :visible.sync="dialogVisible"
               @open="handleOpen"
               @close="handleClose"
               size="tiny">
        <el-tabs v-model="activeName" @tab-click="handleTabClick" v-loading="loading">
            <el-tab-pane :label="L('RoleProperties')" name="first">
                <el-form :model="currRole" :rules="rules" ref="form" label-width="100px" class="demo-ruleForm">
                    <el-form-item :label="L('RoleName')" prop="displayName">
                        <el-input v-model="currRole.displayName"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('Default')" prop="isDefault">
                        <el-checkbox v-model="currRole.isDefault"></el-checkbox>
                        &nbsp; <i style="font-size: 12px">{{L('DefaultRole_Description')}}</i>
                    </el-form-item>
                </el-form>
            </el-tab-pane>

            <el-tab-pane :label="L('Permissions')" name="second" style="min-height: 100px">
                <PermissionTree :context.sync="permissionVm" :orignPermission="permissions"></PermissionTree>
            </el-tab-pane>
        </el-tabs>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
            <el-button type="primary" @click="save">{{L('Save')}}</el-button>
         </span>
    </el-dialog>
</template>

<script>
    import clone from 'clone'
    import roleService from '../../../services/administration/roleService'
    import PermissionTree from '../../../components/tree/PermissionCheck.vue'
    export default {
        props: {
            visible: Boolean,
            role: Object,
            onSave: Function, // 保存成功回调
        },
        data() {
            return {
                activeName: 'first',
                dialogVisible: this.visible,
                loading: false,
                orignRole: void 0,
                currRole: clone(this.role || {}),
                permissions: void 0,
                title: '',
                rules: {displayName: [{required: true, message: lang.L('RoleName'), trigger: 'change'}]},
                permissionVm: void 0
            }
        },
        watch: {
            'role'(val) {
                this.currRole = clone(val)
                this.orignRole = val
            },
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            }
        },
        created() {
        },
        activated() {
        },
        methods: {
            async handleOpen () {
                this.loading = true
                this.permissions = await roleService.getRoleForEdit({id: this.currRole.id})
                this.loading = false
            },
            async handleTabClick(item) {
            },
            handleClose () {
                this.activeName = 'first'
            },
            // 保存
            async save () {
                this.loading = true
                try {
                    let grantedPermissionNames = this.permissionVm.get_selected()
                    await roleService.createOrUpdateRole({
                        role: this.currRole,
                        grantedPermissionNames
                    })

                    this.dialogVisible = false
                    abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))

                    this.$emit('update:role', this.currRole)
                    Object.assign(this.orignRole, this.currRole)
                    this.onSave && this.onSave(this.currRole.id ? 'edit' : 'add')
                } finally {
                    this.loading = false
                }
            }
        },
        components: {
            PermissionTree
        }
    }
</script>
