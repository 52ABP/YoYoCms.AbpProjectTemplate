/**
 * Created by huanghx on 2017/6/25.
 */
const namePre = 'Administration.'
export default {
    path: '/administration',
    name: 'Administration',
    component: resolve => {
        require.ensure([], () => {
            resolve(require('../../views/administration/Index.vue'))
        })
    },
    children: [{ // 组织结构
        path: 'organizationunits',
        name: namePre + 'OrganizationUnits',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/organizationUnits.vue'))
            })
        },
        meta: {
            navName: 'organizationunits'
        },
    }, { // 角色信息
        path: 'roles',
        name: namePre + `Roles`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Roles.vue'))
            })
        }
    }, { // 用户信息
        path: 'users',
        name: namePre + `Users`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Users.vue'))
            })
        }
    }, { // 语言列表
        path: 'languages',
        name: namePre + `Languages`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Language.vue'))
            })
        }
    }, { // 语言文本信息
        path: 'languagestext/:lang',
        name: namePre + `languagestext`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/LanguageTextInfo.vue'))
            })
        },
        meta: {
            displayName: lang.L('LanguageTexts')
        }
    }, { // 审计日志
        path: 'audit-logs',
        name: namePre + `AuditLogs`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/AuditLogs.vue'))
            })
        }
    }, { // Maintenance
        path: 'maintenance',
        name: namePre + `Maintenance`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Maintenance.vue'))
            })
        }
    }, { // 设置
        path: 'settings',
        name: namePre + `Settings.Tenant`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Settting.vue'))
            })
        }
    }]
}
