<!--重置密码-->
<style rel="styleesheet" lang="scss">
    .resetpassword-container {
        background: #00BCD4;
        position: absolute;
        width: 100%;
        height: 100%;
        .el-form-item__content {
            line-height: 0
        }
    }
</style>
<template>
    <article class="resetpassword-container">
        <article class="login-page">
            <div class="login-box">
                <div class="logo">
                    <a href="javascript:void(0);">Abp<b>ProjectTemplate</b></a>
                    <small>Admin BootStrap Based - Material Design</small>
                </div>
                <div class="card">
                    <div class="body" v-loading="loading">
                        <div class="msg">修改密码</div>
                        <el-form ref="form" :model="fetchParam" :rules="rules" label-width="0"
                                 class="demo-ruleForm">
                            <el-form-item label="" prop="password">
                                <div class="input-group">
                                <span class="input-group-addon">
                                    <i class="material-icons">lock</i>
                                </span>
                                    <div class="form-line">
                                        <input type="password" ref="txtPwd" class="form-control" name="username"
                                               placeholder="请输入新密码"
                                               v-model="fetchParam.password"
                                               required
                                               autofocus>
                                    </div>
                                </div>
                            </el-form-item>

                            <el-form-item prop="passwordRepeat">
                                <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                                    <div class="form-line">
                                        <input type="password" @keyup.enter="submit" v-model="fetchParam.passwordRepeat"
                                               class="form-control" placeholder="确认新密码(核对)"
                                               required>
                                    </div>
                                </div>
                            </el-form-item>
                        </el-form>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                                <!--<input type="checkbox" name="rememberme" id="rememberme"-->
                                <!--class="filled-in chk-col-pink">-->
                                <!--<label for="rememberme">记住我</label>-->
                            </div>
                            <div class="col-xs-4">
                                <button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">提交
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </article>
    </article>
</template>

<script>
    //    import config from '../../common/config'
    import userService from '../../services/userService'
    //    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            let validRepass = (rule, value, callback) => {
                if (value != this.fetchParam.password)
                    callback(new Error('两次输入密码不一致!'))
                else
                    callback()
            }
            return {
                loading: false,
                fetchParam: {
                    password: '',
                    passwordRepeat: '',
                },
                rules: {
                    password: [
                        {type: 'string', required: true, message: '请输入新密码', trigger: 'blur'}
                    ],
                    passwordRepeat: [{validator: this.validRepass, trigger: 'change'}],
                },
                validRepass
            }
        },
        mounted() {
            this.$refs.txtPwd.focus()
        },
        methods: {
            submit () {
                this.$refs.form.validate((valid) => {
                    if (!valid) return
                    this.loading = true
                    let params = Object.assign({}, this.$route.query, this.fetchParam)

                    userService.resetPwd(params)
                    setTimeout(() => {
//                        authUtils.setToken(ret)
                        this.$router.push({name: 'Dashboard.Tenant'})
                        abp.notify.success('登录成功!', '恭喜')
                        this.loading = false
                    }, 5e2)
                })
            },
        },
        components: {}
    }
</script>