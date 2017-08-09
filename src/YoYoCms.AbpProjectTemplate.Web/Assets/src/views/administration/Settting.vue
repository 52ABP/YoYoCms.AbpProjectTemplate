<!--设置-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-settting-container {
        @extend %content-container;

        .tab-general {
            > i {
                font-size: 16px;
                color: #666;
            }
        }

        .el-form {
            h2 {
                font-size: 18px;
                color: #666;
            }

            i.tip {
                color: #999;
                margin-left: 8px;
            }
        }
    }
</style>

<template>
    <article class="administration-settting-container" v-loading="loading">
        <section class="right-top-btnContainer">
            <el-button type="primary" icon="save" @click="saveAll"><i
                    class="material-icons material-icons-small">save</i> {{L('SaveAll')}}
            </el-button>
        </section>

        <el-tabs v-model="activeName" @tab-click="handleTabClick">
            <el-tab-pane :label="L('General')" name="general">
                <i>{{L('Timezone')}}</i>
                <Timezone v-model="general.timezone"></Timezone>
            </el-tab-pane>
            <el-tab-pane :label="L('UserManagement')" name="userManagement" class="tab-general">
                <el-form :model="userManagement" label-position="left" ref="formUser"
                         label-width="260px">
                    <el-form label-width="0">
                        <h2>[{{L('FormBasedRegistration')}}]</h2>
                    </el-form>
                    <el-form-item :label="L('AllowUsersToRegisterThemselves')" prop="name">
                        <el-checkbox v-model="userManagement.allowSelfRegistration"></el-checkbox>
                        <i class="tip">{{L('AllowUsersToRegisterThemselves_Hint')}}</i>
                    </el-form-item>
                    <el-form-item :label="L('NewRegisteredUsersIsActiveByDefault')"
                                  prop="isNewRegisteredUserActiveByDefault">
                        <el-checkbox v-model="userManagement.isNewRegisteredUserActiveByDefault"></el-checkbox>
                        <i class="tip">{{L('NewRegisteredUsersIsActiveByDefault_Hint')}}</i>
                    </el-form-item>
                    <el-form-item :label="L('UseCaptchaOnRegistration')" prop="useCaptchaOnRegistration">
                        <el-checkbox v-model="userManagement.useCaptchaOnRegistration"></el-checkbox>
                    </el-form-item>
                    <el-form label-width="0">
                        <h2>[{{L('OtherSettings')}}]</h2>
                    </el-form>
                    <el-form-item :label="L('EmailConfirmationRequiredForLogin')"
                                  prop="isEmailConfirmationRequiredForLogin">
                        <el-checkbox v-model="userManagement.isEmailConfirmationRequiredForLogin"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <el-tab-pane :label="L('Security')" name="security">
                <el-form :model="security" label-position="left" :rules="securityRules" ref="formSecurity"
                         label-width="160px">
                    <el-form label-width="0">
                        <h2>[{{L('UserLockOut')}}]</h2>
                    </el-form>
                    <el-form-item :label="L('EnableUserAccountLockingOnFailedLoginAttemts')"
                                  prop="userLockOut.isEnabled">
                        <el-checkbox v-model="security.userLockOut.isEnabled"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="security.userLockOut.isEnabled"
                                  :label="L('MaxFailedAccessAttemptsBeforeLockout')"
                                  prop="userLockOut.maxFailedAccessAttemptsBeforeLockout">
                        <el-checkbox v-model="security.userLockOut.maxFailedAccessAttemptsBeforeLockout"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="security.userLockOut.isEnabled"
                                  :label="L('DefaultAccountLockoutDurationAsSeconds')"
                                  prop="userLockOut.defaultAccountLockoutSeconds">
                        <el-checkbox v-model="security.userLockOut.defaultAccountLockoutSeconds"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <el-tab-pane :label="L('EmailSmtp')" name="email">
                <el-form :model="email" label-position="left" :rules="emailRules" ref="formEmail" label-width="160px">
                    <el-form-item :label="L('DefaultFromAddress')" prop="defaultFromAddress">
                        <el-input v-model="email.defaultFromAddress"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('DefaultFromDisplayName')" prop="defaultFromDisplayName">
                        <el-input v-model="email.defaultFromDisplayName"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('SmtpHost')" prop="smtpHost">
                        <el-input v-model="email.smtpHost"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('SmtpPort')" prop="smtpPort">
                        <el-input v-model="email.smtpPort"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('UseSsl')" prop="smtpEnableSsl">
                        <el-checkbox v-model="email.smtpEnableSsl"></el-checkbox>
                    </el-form-item>
                    <el-form-item :label="L('UseDefaultCredentials')" prop="smtpUseDefaultCredentials">
                        <el-checkbox v-model="email.smtpUseDefaultCredentials"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" :label="L('DomainName')" prop="smtpDomain">
                        <el-input v-model="email.smtpDomain"></el-input>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" :label="L('UserName')" prop="smtpUserName">
                        <el-input v-model="email.smtpUserName"></el-input>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" :label="L('Password')" prop="smtpPassword">
                        <el-input type="password" v-model="email.smtpPassword"></el-input>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
        </el-tabs>
    </article>
</template>

<script>
    import Timezone from '../../components/select/TimeZone.vue'
    import tenantSettingsService from '../../services/administration/tenantSettingsService'
    export default {
        data() {
            return {
                loading: false,
                activeName: 'general',
                general: {},
                userManagement: {},
                security: {userLockOut: {}},
                email: {},

                userRules: {},
                securityRules: {},
                emailRules: {}
            }
        },
        created() {
        },
        activated() {
//            this.fetchData()
        },
        methods: {
            async fetchData () {
                this.loading = true
                let ret = await tenantSettingsService.getAllSettings()
                this.general = ret.general
                this.userManagement = ret.userManagement
                this.security = ret.security
                this.email = ret.email

                this.loading = false
                abp.view.setContentLoading(false)
            },
            handleTabClick(tab) {
            },
            async saveAll() {
                try {
                    this.loading = true
                    await tenantSettingsService.updateAllSettings({
                        email: this.email,
                        security: this.security,
                        userManagement: this.userManagement,
                        general: this.general
                    })
                    abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                } finally {
                    this.loading = false
                }
            }
        },
        components: {
            Timezone
        }
    }
</script>
