<!--修改密码弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            :title="L('ChangePassword')"
            :visible.sync="dialogVisible"
            size="tiny">
        <el-form :model="fetchParam" :rules="rules" ref="form" label-width="120px">
            <el-form-item :label="L('CurrentPassword')" prop="currentPassword">
                <el-input type="password" :autofocus="true" v-model="fetchParam.currentPassword"
                          :placeholder="L('CurrentPassword')"></el-input>
            </el-form-item>
            <el-form-item :label="L('NewPassword')" prop="newPassword">
                <el-input type="password" v-model="fetchParam.newPassword" :placeholder="L('NewPassword')"></el-input>
            </el-form-item>
            <el-form-item :label="L('NewPasswordRepeat')" prop="newPasswordRepeat">
                <el-input type="password" v-model="fetchParam.newPasswordRepeat" :placeholder="L('NewPasswordRepeat')"></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
            <el-button type="primary" @click="save">
                <i class="material-icons" style="font-size: 14px;vertical-align: middle">save</i>{{L('Save')}}</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import profileService from '../../services/administration/profileService'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            let validRepass = (rule, value, callback) => {
                // debugger
                if (value !== this.fetchParam.newPassword)
                    callback(new Error(lang.L('RepeatPasswordError')))
                else
                    callback()
            }
            return {
                fetchParam: {
                    currentPassword: void 0,
                    newPassword: void 0,
                    newPasswordRepeat: void 0,
                },
                dialogVisible: false,
                rules: {
                    currentPassword: [{required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
                    newPassword: [{required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
                    newPasswordRepeat: [{validator: this.validRepass, trigger: 'change'},
                        {required: true, message: lang.L('RequiredFiled'), trigger: 'change'}]
                },
                validRepass
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
                // 重置
                if (!val) {
                    this.$refs.form.resetFields()
                }
            },
        },
        created() {
        },
        activated() {
        },
        methods: {
            save() {
                this.$refs.form.validate(async (valid) => {
                    if (!valid) return
                    try {
                        await profileService.changePassword(this.fetchParam)
                        this.dialogVisible = false
                        abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                    } finally {
                    }
                })
            }
        },
        components: {}
    }
</script>
