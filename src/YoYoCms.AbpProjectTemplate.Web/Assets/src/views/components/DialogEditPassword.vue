<!--修改密码弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            title="修改密码"
            :visible.sync="dialogVisible"
            size="tiny">
        <el-form :model="fetchParam" :rules="rules" ref="form" label-width="120px">
            <el-form-item label="当前密码" prop="currentPassword">
                <el-input type="password" :autofocus="true" v-model="fetchParam.currentPassword"
                          placeholder="当前密码"></el-input>
            </el-form-item>
            <el-form-item label="新密码" prop="newPassword">
                <el-input type="password" v-model="fetchParam.newPassword" placeholder="新密码"></el-input>
            </el-form-item>
            <el-form-item label="新密码(核对)" prop="newPasswordRepeat">
                <el-input type="password" v-model="fetchParam.newPasswordRepeat" placeholder="重复密码"></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="save">
                <i class="material-icons" style="font-size: 14px;vertical-align: middle">save</i> 保 存</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import profileService from '../../services/profileService'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            let validRepass = (rule, value, callback) => {
                debugger
                if (value !== this.fetchParam.newPassword)
                    callback(new Error('两次密码输入不一致!'))
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
                    currentPassword: [{required: true, message: '请输入当前密码', trigger: 'change'}],
                    newPassword: [{required: true, message: '请输入新密码', trigger: 'change'}],
                    newPasswordRepeat: [{validator: this.validRepass, trigger: 'change'},
                        {required: true, message: '请再次输入密码', trigger: 'change'}]
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
                        abp.notify.success('修改成功', '恭喜')
                    } finally {
                    }
                })
            }
        },
        components: {}
    }
</script>
