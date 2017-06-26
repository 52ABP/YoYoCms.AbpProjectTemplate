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
    children: [{
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
    }, {
        path: 'roles',
        name: namePre + `Roles`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Roles.vue'))
            })
        }
    }, {
        path: 'users',
        name: namePre + `Users`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Users.vue'))
            })
        }
    }]
}
