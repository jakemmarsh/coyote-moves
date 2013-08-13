using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoyoteMoves.Controllers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CoyoteMoves.Data_Access;

namespace CoyoteMovesTest
{
    [TestClass]
    public class ControllerTests
    {
        RequestFormController _controller;
        JObject _jsonObject;
        InfoValidator _validator;

        [TestInitialize]
        public void setup()
        {
            string json = @"{'name':'Brandon DSouza',
                        'deskOrientation':0,
                        'current':{
                            'bazookaInfo':
                                { 'jobTitle':'Intern',
                                'department':'IT',
                                'group':'Unknown',
                                'managerId':'Joe Kolodz',
                                'jobTemplate':'IT Intern'},
                            'ultiproInfo':
                                {'jobTitle':'Intern',
                                'department':'IT',
                                'group':'Unknown',
                                'supervisor':'Joe Kolodz',
                                'other':''},
                            'deskInfo':
                                {'deskNumber':'5-13',
                                'office':'34658df'},
                            'phoneInfo':
                                {'phoneNumber':'|1|8472357349|7349|'}
                        },
                        '$$hashKey':'007',
                        'future':
                            {'bazookaInfo':
                                {'jobTitle':'President',
                                'department':'Human Resources',
                                'group':'T7',
                                'managerId':'Joe Kolodz',
                                'jobTemplate':'3476sdf',
                                'securityItemRights':'324dfgrt'},
                            'deskInfo':
                                {'deskNumber':'sdf345345',
                                'office':'df3445645'},
                            'ultiproInfo':
                                {'jobTitle':'sdf',
                                'department':'5dfsdf',
                                'supervisor': 'Joe Kolodz',
                                'other':'sdf23342'},
                            'phoneInfo':
                                {'phoneNumber':'4234234234234'}},
                        'emailInfo':
                            {'groupsToBeAddedTo':'fdsf34545vf',
                            'groupsToBeRemovedFrom':'345fgf'},
                        'ReviewInfo':
                            {'filesToBeAddedTo':'sdfsdfgf',
                             'filesToBeRemovedFrom':'34345'}}";

            _jsonObject = JObject.Parse(json);
           _controller = new RequestFormController();
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void RequestFormSuccess()
        {

            var response = _controller.SendChangeRequest(_jsonObject);
            Assert.AreEqual(response, "200");

        }
    }
}
