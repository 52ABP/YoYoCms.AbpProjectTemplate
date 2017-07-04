/**
 * Created by huanghx on 2017/7/3.
 */
export default [
    {
        path: '/login',
        name: 'login',
        component: resolve => {
            require.ensure([],
                () => {
                    resolve(require('../../views/loginregist/Login.vue'))
                })
        },
        meta: {
            notAuth: true, // 不需要权限验证
        }
    },
    { // 重置密码
        path: '/resetpassword',
        name: 'resetpassword',
        component: resolve => {
            require.ensure([],
                () => {
                    resolve(require('../../views/loginregist/ResetPassword.vue'))
                })
        },
        meta: {
            notAuth: true, // 不需要权限验证
        }
    },
    { // 注册
        path: '/register',
        name: 'register',
        component: resolve => {
            require.ensure([],
                () => {
                    resolve(require('../../views/loginregist/Regist.vue'))
                })
        },
        meta: {
            notAuth: true, // 不需要权限验证
        }
    },
    { // 忘记密码
        path: '/forgetpwd',
        name: 'forgetPwd',
        component: resolve => {
            require.ensure([],
                () => {
                    resolve(require('../../views/loginregist/ForgetPwd.vue'))
                })
        },
        meta: {
            notAuth: true, // 不需要权限验证
        }
    },
    { // 发送激活邮件
        path: '/sendactiveemail',
        name: 'SendActiveEmail',
        component: resolve => {
            require.ensure([],
                () => {
                    resolve(require('../../views/loginregist/SendActiveEmail.vue'))
                })
        },
        meta: {
            notAuth: true, // 不需要权限验证
        }
    },
]