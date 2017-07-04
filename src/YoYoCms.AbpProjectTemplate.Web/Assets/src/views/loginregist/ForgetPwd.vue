<!--忘记密码-->
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
                        <div class="msg">忘记密码</div>
                        <div class="row">
                            <i class="col-xs-12">
                                <input type="email" @keyup.enter="submit" v-model="fetchParam.emailAddress"
                                       class="form-control" placeholder="邮箱地址" ref="txtEmail">
                            </i>
                        </div>
                        <div class="row">
                            <!--<div class="col-xs-8 p-t-5">-->
                            <!--<button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">返回-->
                            <!--</button>-->
                            <!--</div>-->
                            <div class="col-xs-4 col-xs-offset-8">
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
            return {
                loading: false,
                fetchParam: {
                    emailAddress: '',
                },
            }
        },
        mounted() {
            this.$refs.txtEmail.focus()
        },
        methods: {
            async submit () {
                this.loading = true

                try {
                    await userService.sendPasswordResetLink(this.fetchParam)
                    abp.message.confirm('已向您的邮箱发送了一封密码重置邮件，请查收', '邮件已发送', (ret) => {
                        if (ret) this.$router.replace({name: 'login'})
                    })
                } finally {
                    this.loading = false
                }
            },
        },
        components: {}
    }
</script>