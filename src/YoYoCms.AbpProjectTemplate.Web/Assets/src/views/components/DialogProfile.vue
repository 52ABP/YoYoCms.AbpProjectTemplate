<!--首页修改个人信息的弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            :title="L('MySettings')"
            :visible.sync="dialogVisible"
            size="tiny">
        <el-form :model="user" :rules="rules" ref="form" label-width="100px">
            <el-form-item :label="L('UserName')" prop="userName">
                <el-input v-model="user.userName" :placeholder="L('UserName')" :disabled="true"></el-input>
            </el-form-item>
            <el-form-item :label="L('Name')" prop="name">
                <el-input v-model="user.name"></el-input>
            </el-form-item>
            <el-form-item :label="L('Surname')" prop="surname">
                <el-input v-model="user.surname"></el-input>
            </el-form-item>
            <el-form-item :label="L('EmailAddress')" prop="emailAddress">
                <el-input v-model="user.emailAddress"></el-input>
            </el-form-item>
            <el-form-item :label="L('PhoneNumber')" prop="phoneNumber">
                <el-input v-model="user.phoneNumber" :placeholder="L('PhoneNumber')"></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
            <el-button class="waves-effect" type="primary" @click="save">
                <i class="material-icons" style="font-size: 14px;vertical-align: middle">save</i>{{L('Save')}}</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import profileService from '../../services/administration/profileService'
    import clone from 'clone'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            return {
                user: {},
                dialogVisible: false,
                rules: {
                    name: [{required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
                    surname: [{required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
                    emailAddress: [{required: true, message: lang.L('RequiredFiled'), trigger: 'change'}]
                },
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                if (val) this.user = clone(this.$store.state.auth.user)
                this.$emit('update:visible', val)
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
                        await profileService.updateCurrentUserProfile(this.user)

                        this.$store.dispatch('setAuthUser', {user: this.user})
                        this.dialogVisible = false
                    } finally {
                    }
                })
            }
        },
        components: {}
    }
</script>
