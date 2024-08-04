﻿open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Giraffe.EndpointRouting

let handler1: HttpHandler =
  fun (_: HttpFunc) (ctx: HttpContext) -> ctx.WriteTextAsync "Hello World"

let handler2 (firstName: string, age: int) : HttpHandler =
  fun (_: HttpFunc) (ctx: HttpContext) ->
    $"Hello %s{firstName}, you are %i{age} years old." |> ctx.WriteTextAsync

let handler3 (a: string, b: string, c: string, d: int) : HttpHandler =
  fun (_: HttpFunc) (ctx: HttpContext) ->
    $"Hello %s{a} %s{b} %s{c} %i{d}" |> ctx.WriteTextAsync

let endpoints =
  [ subRoute "/foo" [ GET [ route "/bar" (text "Aloha!") ] ]
    GET
      [ route "/" (text "Hello World")
        routef "/%s/%i" handler2
        routef "/%s/%s/%s/%i" handler3 ]
    GET_HEAD
      [ route "/foo" (text "Bar")
        route "/x" (text "y")
        route "/abc" (text "def")
        route "/123" (text "456") ]
    // Not specifying a http verb means it will listen to all verbs
    subRoute "/sub" [ route "/test" handler1 ] ]

let notFoundHandler = "Not Found" |> text |> RequestErrors.notFound

let configureApp (appBuilder: IApplicationBuilder) =
  appBuilder.UseRouting().UseGiraffe(endpoints).UseGiraffe(notFoundHandler)

let configureServices (services: IServiceCollection) =
  services.AddRouting().AddGiraffe() |> ignore

[<EntryPoint>]
let main args =
  let builder = WebApplication.CreateBuilder(args)
  configureServices builder.Services

  let app = builder.Build()

  if app.Environment.IsDevelopment() then
    app.UseDeveloperExceptionPage() |> ignore

  configureApp app
  app.Run()

  0
