﻿// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Datasync.Common.Test;
using Datasync.Common.Test.Models;
using FluentAssertions;
using Microsoft.Datasync.Client.Query;
using Microsoft.Datasync.Client.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Datasync.Client.Test.Query
{
    [ExcludeFromCodeCoverage]
    public class TableQuery_Tests : BaseTest
    {
        [Fact]
        [Trait("Method", "Ctor")]
        public void Ctor_NullTable_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TableQuery<IdEntity>(null));
        }

        [Fact]
        [Trait("Method", "Ctor")]
        public void Ctor_BlankSetup()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Assert.Same(table, query.Table);
            Assert.IsAssignableFrom<IQueryable<IdEntity>>(query.Query);
            Assert.Empty(query.QueryParameters);
            Assert.Equal(0, query.SkipCount);
            Assert.Equal(0, query.TakeCount);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        public void ToODataString_BlankQuery_ReturnsEmptyString()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Empty(query.ToQueryString());
        }

        [Fact]
        [Trait("Method", "IncludeDeletedItems")]
        public void IncludeDeletedItems_Enabled_ChangesKey()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            query.QueryParameters.Add("__includedeleted", "test");

            var actual = query.IncludeDeletedItems() as TableQuery<IdEntity>;

            AssertEx.Contains("__includedeleted", "true", actual.QueryParameters);
        }

        [Fact]
        [Trait("Method", "IncludeDeletedItems")]
        public void IncludeDeletedItems_Disabled_RemovesKey()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            query.QueryParameters.Add("__includedeleted", "true");

            var actual = query.IncludeDeletedItems(false) as TableQuery<IdEntity>;

            Assert.False(actual.QueryParameters.ContainsKey("__includedeleted"));
        }

        [Fact]
        [Trait("Method", "IncludeDeletedItems")]
        public void IncludeDeletedItems_Disabled_WorksWithEmptyParameters()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.IncludeDeletedItems(false) as TableQuery<IdEntity>;

            Assert.False(actual.QueryParameters.ContainsKey("__includedeleted"));
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "IncludeDeletedItems")]
        public void ToQueryString_IncludeDeletedItems_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).IncludeDeletedItems() as TableQuery<IdEntity>;

            var odata = query.ToQueryString();
            Assert.Equal("__includedeleted=true", odata);
        }

        [Fact]
        [Trait("Method", "IncludeTotalCount")]
        public void IncludeTotalCount_Enabled_AddsKey()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).IncludeTotalCount(true) as TableQuery<IdEntity>;

            AssertEx.Contains("$count", "true", query.QueryParameters);
        }

        [Fact]
        [Trait("Method", "IncludeTotalCount")]
        public void IncludeTotalCount_Enabled_ChangesKey()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            query.QueryParameters.Add("$count", "test");

            var actual = query.IncludeTotalCount() as TableQuery<IdEntity>;

            AssertEx.Contains("$count", "true", actual.QueryParameters);
        }

        [Fact]
        [Trait("Method", "IncludeTotalCount")]
        public void IncludeTotalCount_Disabled_RemovesKey()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            query.QueryParameters.Add("$count", "true");

            var actual = query.IncludeTotalCount(false) as TableQuery<IdEntity>;

            Assert.False(actual.QueryParameters.ContainsKey("$count"));
        }

        [Fact]
        [Trait("Method", "IncludeTotalCount")]
        public void IncludeTotalCount_Disabled_WorksWithEmptyParameters()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.IncludeTotalCount(false) as TableQuery<IdEntity>;

            Assert.False(actual.QueryParameters.ContainsKey("$count"));
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "IncludeTotalCount")]
        public void ToQueryString_IncludeTotalCount_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).IncludeTotalCount() as TableQuery<IdEntity>;

            var odata = query.ToQueryString();
            Assert.Equal("$count=true", odata);
        }

        [Fact]
        [Trait("Method", "OrderBy")]
        public void OrderBy_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, string>> keySelector = null;

            Assert.Throws<ArgumentNullException>(() => query.OrderBy(keySelector));
        }

        [Fact]
        [Trait("Method", "OrderBy")]
        public void OrderBy_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.OrderBy(m => m.Id) as TableQuery<IdEntity>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("OrderBy", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "OrderBy")]
        public void ToQueryString_OrderBy_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).OrderBy(m => m.Id) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("$orderby=id", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "OrderBy")]
        public void ToQueryString_OrderBy_ThrowsNotSupported()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).OrderBy(m => m.Id.ToLower()) as TableQuery<IdEntity>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        [Trait("Method", "OrderByDescending")]
        public void OrderByDescending_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, string>> keySelector = null;

            Assert.Throws<ArgumentNullException>(() => query.OrderByDescending(keySelector));
        }

        [Fact]
        [Trait("Method", "OrderByDescending")]
        public void OrderByDescending_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.OrderByDescending(m => m.Id) as TableQuery<IdEntity>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("OrderByDescending", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "OrderByDescending")]
        public void ToQueryString_OrderByDescending_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).OrderByDescending(m => m.Id) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("$orderby=id desc", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "OrderByDescending")]
        public void ToQueryString_OrderByDescending_ThrowsNotSupported()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).OrderByDescending(m => m.Id.ToLower()) as TableQuery<IdEntity>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        [Trait("Method", "Select")]
        public void Select_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, IdOnly>> selector = null;

            Assert.Throws<ArgumentNullException>(() => query.Select(selector));
        }

        [Fact]
        [Trait("Method", "Select")]
        public void Select_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.Select(m => new IdOnly { Id = m.Id }) as TableQuery<IdOnly>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("Select", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "Select")]
        public void ToQueryString_Select_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).Select(m => new IdOnly { Id = m.Id }) as TableQuery<IdOnly>;

            var odata = query.ToQueryString();

            Assert.Equal("$select=id", odata);
        }

        [Theory, CombinatorialData]
        [Trait("Method", "Skip")]
        public void Skip_Throws_OutOfRange([CombinatorialValues(-10, -1)] int skip)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Throws<ArgumentOutOfRangeException>(() => query.Skip(skip));
        }

        [Fact]
        [Trait("Method", "Skip")]
        public void Skip_Sets_SkipCount()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.Skip(5) as TableQuery<IdEntity>;

            Assert.Equal(5, actual.SkipCount);
        }

        [Fact]
        [Trait("Method", "Skip")]
        public void Skip_IsCumulative()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.Skip(5).Skip(20) as TableQuery<IdEntity>;

            Assert.Equal(25, actual.SkipCount);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "Skip")]
        public void ToQueryString_Skip_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).Skip(5) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();
            Assert.Equal("$skip=5", odata);
        }

        [Theory, CombinatorialData]
        [Trait("Method", "Take")]
        public void Take_ThrowsOutOfRange([CombinatorialValues(-10, -1, 0)] int take)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Throws<ArgumentOutOfRangeException>(() => query.Take(take));
        }

        [Theory]
        [InlineData(5, 2, 2)]
        [InlineData(2, 5, 2)]
        [InlineData(5, 20, 5)]
        [InlineData(20, 5, 5)]
        [Trait("Method", "Take")]
        public void Take_MinimumWins(int first, int second, int expected)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.Take(first).Take(second) as TableQuery<IdEntity>;

            Assert.Equal(expected, actual.TakeCount);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "Take")]
        public void ToQueryString_Take_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).Take(5) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();
            Assert.Equal("$top=5", odata);
        }

        [Fact]
        [Trait("Method", "ThenBy")]
        public void ThenBy_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, string>> keySelector = null;

            Assert.Throws<ArgumentNullException>(() => query.ThenBy(keySelector));
        }

        [Fact]
        [Trait("Method", "ThenBy")]
        public void ThenBy_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.ThenBy(m => m.Id) as TableQuery<IdEntity>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("ThenBy", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "ThenBy")]
        public void ToQueryString_ThenBy_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).ThenBy(m => m.Id) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("$orderby=id", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "ThenBy")]
        public void ToQueryString_ThenBy_ThrowsNotSupported()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).ThenBy(m => m.Id.ToLower()) as TableQuery<IdEntity>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        [Trait("Method", "ThenByDescending")]
        public void ThenByDescending_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, string>> keySelector = null;

            Assert.Throws<ArgumentNullException>(() => query.ThenByDescending(keySelector));
        }

        [Fact]
        [Trait("Method", "ThenByDescending")]
        public void ThenByDescending_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.ThenByDescending(m => m.Id) as TableQuery<IdEntity>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("ThenByDescending", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "ThenByDescending")]
        public void ToQueryString_ThenByDescending_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).ThenByDescending(m => m.Id) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("$orderby=id desc", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "ThenByDescending")]
        public void ToQueryString_ThenByDescending_ThrowsNotSupported()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).ThenByDescending(m => m.Id.ToLower()) as TableQuery<IdEntity>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        [Trait("Method", "ToAsyncPageable")]
        public async Task ToAsyncPageable_WithCount_Executes()
        {
            // Arrange
            var client = GetMockClient();
            var sEndpoint = new Uri(Endpoint, "/tables/movies/").ToString();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var page1 = CreatePageOfItems(5, 10, new Uri($"{sEndpoint}?page=2"));
            var page2 = CreatePageOfItems(5, 10, new Uri($"{sEndpoint}?page=3"));
            MockHandler.AddResponse(HttpStatusCode.OK, new Page<IdEntity>());
            List<IdEntity> items = new();
            var query = new TableQuery<IdEntity>(table).IncludeTotalCount();
            long? count = null;

            // Act
            var pageable = query.ToAsyncPageable();
            var enumerator = pageable.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (!count.HasValue) count = pageable.Count;
                items.Add(enumerator.Current);
            }

            // Assert - request
            Assert.Equal(3, MockHandler.Requests.Count);
            var request = MockHandler.Requests[0];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?$count=true", request.RequestUri.ToString());

            request = MockHandler.Requests[1];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?page=2", request.RequestUri.ToString());

            request = MockHandler.Requests[2];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?page=3", request.RequestUri.ToString());

            // Assert - response
            Assert.Equal(10, count);
            Assert.NotNull(pageable.CurrentResponse);
            Assert.Equal(200, pageable.CurrentResponse.StatusCode);
            Assert.True(pageable.CurrentResponse.HasContent);
            Assert.NotEmpty(pageable.CurrentResponse.Content);

            Assert.Equal(10, items.Count);
            Assert.Equal(page1.Items, items.Take(5));
            Assert.Equal(page2.Items, items.Skip(5).Take(5));
        }

        [Fact]
        [Trait("Method", "ToAsyncPageable")]
        public async Task ToAsyncPageable_WithQuery_Executes()
        {
            // Arrange
            var client = GetMockClient();
            var sEndpoint = new Uri(Endpoint, "/tables/movies/").ToString();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var page1 = CreatePageOfItems(5, 10, new Uri($"{sEndpoint}?page=2"));
            var page2 = CreatePageOfItems(5, 10, new Uri($"{sEndpoint}?page=3"));
            MockHandler.AddResponse(HttpStatusCode.OK, new Page<IdEntity>());
            List<IdEntity> items = new();
            var query = new TableQuery<IdEntity>(table).Where(m => m.StringValue == "foo").IncludeTotalCount();
            long? count = null;

            // Act
            var pageable = query.ToAsyncPageable();
            var enumerator = pageable.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (!count.HasValue) count = pageable.Count;
                items.Add(enumerator.Current);
            }

            // Assert - request
            Assert.Equal(3, MockHandler.Requests.Count);
            var request = MockHandler.Requests[0];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?$count=true&$filter=(stringValue eq 'foo')", request.RequestUri.ToString());

            request = MockHandler.Requests[1];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?page=2", request.RequestUri.ToString());

            request = MockHandler.Requests[2];
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal($"{sEndpoint}?page=3", request.RequestUri.ToString());

            // Assert - response
            Assert.Equal(10, count);
            Assert.NotNull(pageable.CurrentResponse);
            Assert.Equal(200, pageable.CurrentResponse.StatusCode);
            Assert.True(pageable.CurrentResponse.HasContent);
            Assert.NotEmpty(pageable.CurrentResponse.Content);
        }

        [Fact]
        [Trait("Method", "Where")]
        public void Where_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            Expression<Func<IdEntity, bool>> predicate = null;

            Assert.Throws<ArgumentNullException>(() => query.Where(predicate));
        }

        [Fact]
        [Trait("Method", "Where")]
        public void Where_UpdatesQuery()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var actual = query.Where(m => m.Id.Contains("foo")) as TableQuery<IdEntity>;

            Assert.IsAssignableFrom<MethodCallExpression>(actual.Query.Expression);
            var expression = actual.Query.Expression as MethodCallExpression;
            Assert.Equal("Where", expression.Method.Name);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "Where")]
        public void ToQueryString_Where_IsWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).Where(m => m.Id == "foo") as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("$filter=(id%20eq%20'foo')", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "Where")]
        public void ToQueryString_Where_ThrowsNotSupported()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).ThenByDescending(m => m.Id.Normalize() == "foo") as TableQuery<IdEntity>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Theory]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        [Trait("Method", "WithParameter")]
        public void WithParameter_Null_Throws(string key, string value)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Throws<ArgumentNullException>(() => query.WithParameter(key, value));
        }

        [Theory]
        [InlineData("testkey", "")]
        [InlineData("testkey", " ")]
        [InlineData("testkey", "   ")]
        [InlineData("testkey", "\t")]
        [InlineData("", "testvalue")]
        [InlineData(" ", "testvalue")]
        [InlineData("   ", "testvalue")]
        [InlineData("\t", "testvalue")]
        [InlineData("$count", "true")]
        [InlineData("__includedeleted", "true")]
        [Trait("Method", "WithParameter")]
        public void WithParameter_Illegal_Throws(string key, string value)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Throws<ArgumentException>(() => query.WithParameter(key, value));
        }

        [Fact]
        [Trait("Method", "WithParameter")]
        public void WithParameter_SetsParameter()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            var actual = query.WithParameter("testkey", "testvalue") as TableQuery<IdEntity>;

            AssertEx.Contains("testkey", "testvalue", actual.QueryParameters);
        }

        [Fact]
        [Trait("Method", "WithParameter")]
        public void WithParameter_Overwrites()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).WithParameter("testkey", "testvalue");

            var actual = query.WithParameter("testkey", "replacement") as TableQuery<IdEntity>;

            AssertEx.Contains("testkey", "replacement", actual.QueryParameters);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "WithParameter")]
        public void ToQueryString_WithParameter_isWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).WithParameter("testkey", "testvalue") as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("testkey=testvalue", odata);
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "WithParameter")]
        public void ToQueryString_WithParameter_EncodesValue()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).WithParameter("testkey", "test value") as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("testkey=test%20value", odata);
        }

        [Fact]
        [Trait("Method", "WithParameters")]
        public void WithParameters_Null_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);

            Assert.Throws<ArgumentNullException>(() => query.WithParameters(null));
        }

        [Fact]
        [Trait("Method", "WithParameters")]
        public void WithParameters_Empty_Throws()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentException>(() => query.WithParameters(sut));
        }

        [Fact]
        [Trait("Method", "WithParameters")]
        public void WithParameters_CopiesParams()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var sut = new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };

            var actual = query.WithParameters(sut) as TableQuery<IdEntity>;

            AssertEx.Contains("key1", "value1", actual.QueryParameters);
            AssertEx.Contains("key2", "value2", actual.QueryParameters);
        }

        [Fact]
        [Trait("Method", "WithParameters")]
        public void WithParameters_MergesParams()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table).WithParameter("key1", "value1");
            var sut = new Dictionary<string, string>()
            {
                { "key1", "replacement" },
                { "key2", "value2" }
            };

            var actual = query.WithParameters(sut) as TableQuery<IdEntity>;

            AssertEx.Contains("key1", "replacement", actual.QueryParameters);
            AssertEx.Contains("key2", "value2", actual.QueryParameters);
        }

        [Theory]
        [InlineData("$count")]
        [InlineData("__includedeleted")]
        [Trait("Method", "WithParameters")]
        public void WithParameters_CannotSetIllegalParams(string key)
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var query = new TableQuery<IdEntity>(table);
            var sut = new Dictionary<string, string>()
            {
                { key, "true" },
                { "key2", "value2" }
            };

            Assert.Throws<ArgumentException>(() => query.WithParameters(sut));
        }

        [Fact]
        [Trait("Method", "ToQueryString")]
        [Trait("Method", "WithParameters")]
        public void ToQueryString_WithParameters_isWellFormed()
        {
            var client = GetMockClient();
            RemoteTable<IdEntity> table = client.GetRemoteTable<IdEntity>("movies") as RemoteTable<IdEntity>;
            var pairs = new Dictionary<string, string>()
            {
                {  "key1", "value1" },
                {  "key2", "value 2" }
            };
            var query = new TableQuery<IdEntity>(table).WithParameters(pairs) as TableQuery<IdEntity>;

            var odata = query.ToQueryString();

            Assert.Equal("key1=value1&key2=value%202", odata);
        }

        [Theory]
        [ClassData(typeof(LinqTestCases))]
        [Trait("Method", "ToQueryString")]
        internal void LinqODataConversions(LinqTestCase testcase)
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act
            var actual = (testcase.LinqExpression.Invoke(query) as TableQuery<ClientMovie>)?.ToQueryString();
            var tester = Uri.UnescapeDataString(actual);

            // Assert
            Assert.NotNull(actual);

            var expectedParams = testcase.ODataString.Split('&').ToList();
            var actualParams = tester.Split('&').ToList();
            // actualParams and expectedParams need to be the same, but can be in different order
            actualParams.Should().BeEquivalentTo(expectedParams, $"Test Case {testcase.Name} OData String");
        }

        [Theory]
        [ClassData(typeof(LinqTestCases))]
        [Trait("Method", "ToQueryString")]
        internal void LinqODataWithSelectConversions(LinqTestCase testcase)
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Need to make sure the $select statement is added in the right spot.
            var splitArgs = testcase.ODataString.Split('&').ToList();
            splitArgs.Add("$select=id,title");
            splitArgs.Sort();
            var expectedWithSelect = string.Join('&', splitArgs).TrimStart('&');

            // Act
            var actual = (testcase.LinqExpression.Invoke(query).Select(m => new SelectResult { Id = m.Id, Title = m.Title }) as TableQuery<SelectResult>)?.ToQueryString();
            var tester = Uri.UnescapeDataString(actual);

            // Assert
            Assert.NotNull(actual);
            Assert.True(tester.Equals(expectedWithSelect), $"Test '{testcase.Name}' did not match (with select)\nExpected: {expectedWithSelect}\nActual  : {tester}");
        }

        [Fact]
        public void Linq_NotSupportedProperties()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act
            var actual = query.Where(m => m.ReleaseDate.UtcDateTime > new DateTime(2001, 12, 31)) as TableQuery<ClientMovie>;
            Assert.Throws<NotSupportedException>(() => actual.ToQueryString());
        }

        [Fact]
        public void Linq_NotSupportedMethods()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act
            var actual = query.Where(m => m.Title.LastIndexOf("er") > 0) as TableQuery<ClientMovie>;
            Assert.Throws<NotSupportedException>(() => actual.ToQueryString());
        }

        [Fact]
        public void Linq_NotSupportedBinaryOperators()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act
            var actual = query.Where(m => (m.Year ^ 1024) == 0) as TableQuery<ClientMovie>;
            Assert.Throws<NotSupportedException>(() => actual.ToQueryString());
        }

        [Fact]
        public void Linq_NotSupportedUnaryOperators()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act
            var actual = query.Where(m => (5 * (-m.Duration)) > -180) as TableQuery<ClientMovie>;
            Assert.Throws<NotSupportedException>(() => actual.ToQueryString());
        }

        [Fact]
        public void Linq_NotSupportedDistinctLinqStatement()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table);

            // Act - really - you should NOT be doing this!
            query.Query = query.Query.Distinct();

            // Assert
            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        public void Linq_NegateNotSupported()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table).Where(m => (-m.Year) <= -2000) as TableQuery<ClientMovie>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        public void Linq_InvalidOrderBy_Lambda()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table).OrderBy(m => m.Id == "foo" ? "yes" : "no") as TableQuery<ClientMovie>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        public void Linq_InvalidOrderBy_Method()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table).OrderBy(m => m.GetHashCode()) as TableQuery<ClientMovie>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }

        [Fact]
        public void Linq_InvalidOrderBy_ToString()
        {
            // Arrange
            var client = GetMockClient();
            var table = new RemoteTable<ClientMovie>("tables/movies/", client.HttpClient, client.ClientOptions);
            var query = new TableQuery<ClientMovie>(table).OrderBy(m => m.ReleaseDate.ToString("o")) as TableQuery<ClientMovie>;

            Assert.Throws<NotSupportedException>(() => query.ToQueryString());
        }
    }
}